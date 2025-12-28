using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ChatOps.Api.Integrations.Telegram.Handling;

internal sealed class UpdateHandler : IUpdateHandler
{
    private readonly ITelegramMessageHandler _messageHandler;
    private readonly ILogger<UpdateHandler> _logger;

    public UpdateHandler(ITelegramMessageHandler messageHandler,
        ILogger<UpdateHandler> logger)
    {
        _messageHandler = messageHandler;
        _logger = logger;
    }
    
    public async Task HandleUpdateAsync(ITelegramBotClient botClient, 
        Update update, 
        CancellationToken ct)
    {
        using var loggerScope = _logger.BeginScope(new Dictionary<string, object?>
        {
            ["UpdateType"] = update.Type,
            ["MessageType"] = update.Message?.Type,
            ["MessageText"] = update.Message?.Text
        });
        
        if (update.Type != UpdateType.Message)
        {
            _logger.LogInformation("Unsupported update type: {UpdateType}", update.Type);
            return;
        }

        if (update.Message is null)
        {
            _logger.LogInformation("Message is null");
            return;
        }
        
        HandleTelegramMessageResult result;
        try
        {
            result = await _messageHandler.Handle(update.Message, ct);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occured while handling text command '{Text:l}' from message", update.Message.Text);
            return;
        }

        switch (result.Code)
        {
            case HandleTelegramMessageCode.Success:
                _logger.LogInformation("Text command handling successfully");

                if (result.HasResult)
                {
                    _logger.LogInformation("Sending response to chat: {Response:l}", result.Result!);
                    _ = await SendTextMessage(botClient, update.Message.Chat.Id, result.Result!, ct);
                }
                
                break;
            
            case HandleTelegramMessageCode.Failure:
                _logger.LogWarning("Text command handling failure: '{ErrorMessage:l}'", result.Error);
                break;
            
            case HandleTelegramMessageCode.UnknownCommand:
                _logger.LogWarning("Unknown command '{Command:l}'", update.Message.Text);
                break;
            
            default:
                _logger.LogWarning("Unknown text command handling code: '{Code}'", result.Code);
                break;
        }
    }

    public Task HandleErrorAsync(ITelegramBotClient botClient, 
        Exception exception, 
        HandleErrorSource source,
        CancellationToken ct)
    {
        _logger.LogError(exception, "Error '{ErrorSource}' occured while handling telegram message",
            source);
        return Task.CompletedTask;
    }

    private async Task<Message?> SendTextMessage(ITelegramBotClient botClient, 
        long chatId,
        string text, 
        CancellationToken ct)
    {
        try
        {
            return await botClient.SendMessage(chatId: chatId,
                text: text,
                parseMode: ParseMode.Markdown,
                cancellationToken: ct);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occured while sending text message '{TextMessage:l}' to chat",
                text);
            return null;
        }
    }
}