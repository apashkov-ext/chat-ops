using OneOf;

namespace ChatOps.Api.Integrations.Telegram;

internal interface ITelegramChatApi
{
    Task<OneOf<Message, SendTelegramMessageFailure>> SendHtmlMessage(
        long chatId, 
        string text, 
        CancellationToken ct = default);
    
    Task<OneOf<Message, SendTelegramMessageFailure>> SendImage(
        long chatId, 
        Stream image, 
        CancellationToken ct = default);
}

internal sealed class TelegramChatApi : ITelegramChatApi
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<TelegramChatApi> _logger;

    public TelegramChatApi(ITelegramBotClient botClient,
        ILogger<TelegramChatApi> logger)
    {
        _botClient = botClient;
        _logger = logger;
    }

    public async Task<OneOf<Message, SendTelegramMessageFailure>> SendHtmlMessage(
        long chatId, 
        string text, 
        CancellationToken ct = default)
    {
        _logger.LogDebug("Sending html message to chat: {HtmlMessage:l}", text);
        try
        {
            return await _botClient.SendMessage(chatId: chatId,
                text: text,
                parseMode: ParseMode.Html,
                cancellationToken: ct);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occured while sending text message '{TextMessage:l}' to chat '{ChatId}'",
                text,
                chatId);
            return new SendTelegramMessageFailure();
        }
    }

    public async Task<OneOf<Message, SendTelegramMessageFailure>> SendImage(
        long chatId, 
        Stream image, 
        CancellationToken ct = default)
    {
        _logger.LogDebug("Sending image to chat");
        try
        {
            // TODO: можно закэшировать fileId, чтобы не слать в чат заново (не грузить по сети) одну и ту же картинку.
            // Погуглить про это.
            return await _botClient.SendPhoto(chatId: chatId,
                InputFile.FromStream(image),
                parseMode: ParseMode.Html,
                cancellationToken: ct);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occured while sending image to chat '{ChatId}'",
                chatId);
            return new SendTelegramMessageFailure();
        }
    }
}

internal sealed record SendTelegramMessageFailure;