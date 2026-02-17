using ChatOps.Api.Integrations.Telegram.Core;
using ChatOps.Api.LocalAdapters.Files;
using ChatOps.Api.LocalAdapters.Users;
using Telegram.Bot.Polling;

namespace ChatOps.Api.Integrations.Telegram;

internal sealed class UpdateHandler : IUpdateHandler
{
    private readonly ITelegramCommandHandler[] _handlers;
    private readonly IUpdateHandlerGuard _guard;
    private readonly IUpsertTelegramUser _saveTelegramUser;
    private readonly ITelegramChatApi _chatApi;
    private readonly IGetStreamByFileId _getStreamByFileId;
    private readonly ILogger<UpdateHandler> _logger;

    public UpdateHandler(
        IUpdateHandlerGuard guard,
        IEnumerable<ITelegramCommandHandler> handlers,
        IUpsertTelegramUser saveTelegramUser,
        ITelegramChatApi chatApi,
        IGetStreamByFileId getStreamByFileId,
        ILogger<UpdateHandler> logger)
    {
        _handlers = handlers.ToArray();
        _guard = guard;
        _saveTelegramUser = saveTelegramUser;
        _chatApi = chatApi;
        _getStreamByFileId = getStreamByFileId;
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

        _logger.LogInformation("Handling update from chat {ChatId:}", 
            update.Message?.Chat.Id);

        if (!_guard.CanHandle(update))
        {
            _logger.LogDebug("Update cannot be handled, skipping");
            return;
        }

        await Handle(update, ct);
    }

    public Task HandleErrorAsync(ITelegramBotClient botClient, 
        Exception exception, 
        HandleErrorSource source,
        CancellationToken ct)
    {
        _logger.LogError(exception, "Error '{ErrorSource}' occured while handling telegram message", source);
        return Task.CompletedTask;
    }

    private async Task Handle(Update update, CancellationToken ct)
    {
        var message = update.Message;
        if (message is null || !TelegramCommandFactory.TryCreate(message, out var command))
        {
            _logger.LogDebug("This is not a command, skipping");
            return;
        }
        
        // запомним юзера для рендера ответа.
        _saveTelegramUser.Execute(command.User);
        
        if (command.Tokens.Count == 0)
        {
            _logger.LogInformation("Empty command, skipping");
            return;
        }
        
        var handler = _handlers.FirstOrDefault(h => h.CanHandle(command));
        if (handler is null)
        {
            _logger.LogWarning("Unknown command '{Command:l}'", message.Text);
            await _chatApi.SendHtmlMessage(
                message.Chat.Id,
                "⚠️ Неизвестная команда",
                ct);
            return;
        }

        var result = await handler.Handle(command, ct);
        await result.SwitchAsync(
            async reply =>
            {
                _logger.LogInformation("Text command handling successfully");
                
                if (reply.Text is not null)
                {
                    await _chatApi.SendHtmlMessage(message.Chat.Id, reply.Text.Text, ct);
                }

                if (reply.Image is not null)
                {
                    await using var imageStream = _getStreamByFileId.Execute(reply.Image.ImageId);
                    await _chatApi.SendImage(message.Chat.Id, imageStream, ct);
                }
            },
            async failure =>
            {
                _logger.LogWarning("Text command handling failure: '{ErrorMessage:l}'", failure.Error);
                await _chatApi.SendHtmlMessage(
                    message.Chat.Id, 
                    $"⛔ {failure.Error}", 
                    ct);
            },
            async _ =>
            {
                _logger.LogWarning("Unknown command '{Command:l}'", message.Text);
                await _chatApi.SendHtmlMessage(
                    message.Chat.Id,
                    "⚠️ Неизвестная команда",
                    ct);
            }
        );
    }
}