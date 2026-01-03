using ChatOps.Api.Integrations.Telegram.Core;
using Telegram.Bot.Polling;

namespace ChatOps.Api.Integrations.Telegram;

internal sealed class UpdateHandler : IUpdateHandler
{
    private readonly ITelegramCommandHandler[] _handlers;
    private readonly ITelegramChatApi _chatApi;
    private readonly ILogger<UpdateHandler> _logger;

    public UpdateHandler(IEnumerable<ITelegramCommandHandler> handlers,
        ITelegramChatApi chatApi,
        ILogger<UpdateHandler> logger)
    {
        _handlers = handlers.ToArray();
        _chatApi = chatApi;
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

        var message = update.Message;
        if (message is null)
        {
            _logger.LogInformation("Message is null");
            return;
        }
        
        if (message.Type != MessageType.Text)
        {
            _logger.LogInformation("Unsupported message type: {MessageType}", message.Type);
            return;
        }
        
        if (message.From is null)
        {
            _logger.LogInformation("Message.From is null");
            return;
        }
        
        var command = message.Text ?? string.Empty;
        var collection = CommandTokenCollection.Parse(command);
        if (collection.Tokens.Count == 0)
        {
            _logger.LogInformation("Empty command, skipping");
            return;
        }
        
        var handler = _handlers.FirstOrDefault(h => h.CanHandle(collection));
        if (handler is null)
        {
            _logger.LogWarning("Unknown command '{Command:l}'", message.Text);
            await _chatApi.SendHtmlMessage(
                message.Chat.Id,
                "Неизвестная команда",
                ct);
            return;
        }

        var result = await handler.Handle(collection, ct);
        await result.SwitchAsync(
            async reply =>
            {
                _logger.LogInformation("Text command handling successfully");
                await _chatApi.SendHtmlMessage(message.Chat.Id, reply.Text, ct);
            },
            async failure =>
            {
                _logger.LogWarning("Text command handling failure: '{ErrorMessage:l}'", failure.Error);
                await _chatApi.SendHtmlMessage(message.Chat.Id, failure.Error, ct);
            },
            async _ =>
            {
                _logger.LogWarning("Unknown command '{Command:l}'", message.Text);
                await _chatApi.SendHtmlMessage(
                    message.Chat.Id,
                    "Неизвестная команда",
                    ct);
            }
        );
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
}