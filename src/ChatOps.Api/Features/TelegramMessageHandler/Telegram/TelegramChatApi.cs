using OneOf;

namespace ChatOps.Api.Features.TelegramMessageHandler.Telegram;

internal interface ITelegramChatApi
{
    Task<OneOf<Message, SendTelegramMessageFailure>> SendHtmlMessage(long chatId, string text, CancellationToken ct = default);
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

    public async Task<OneOf<Message, SendTelegramMessageFailure>> SendHtmlMessage(long chatId, string text, CancellationToken ct = default)
    {
        _logger.LogInformation("Sending response to chat: {Response:l}", text);
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
}

internal sealed record SendTelegramMessageFailure;