using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ChatOps.Api.Integrations.Telegram;

internal interface ITelegramChatApi
{
    Task<Message?> SendHtmlMessage(long chatId, string text, CancellationToken ct = default);
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

    public async Task<Message?> SendHtmlMessage(long chatId, string text, CancellationToken ct = default)
    {
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
            return null;
        }
    }
}