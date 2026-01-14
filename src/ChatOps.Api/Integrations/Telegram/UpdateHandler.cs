using ChatOps.Api.Integrations.FileStorage;
using ChatOps.Api.Integrations.Telegram.Core;
using Microsoft.Extensions.Options;
using Telegram.Bot.Polling;

namespace ChatOps.Api.Integrations.Telegram;

internal sealed class UpdateHandler : IUpdateHandler
{
    private readonly long[] _allowedChatIds;
    private readonly ITelegramCommandHandler[] _handlers;
    private readonly IUsersCache _usersStore;
    private readonly ITelegramChatApi _chatApi;
    private readonly IImageResolver _imageResolver;
    private readonly ILogger<UpdateHandler> _logger;

    public UpdateHandler(IEnumerable<ITelegramCommandHandler> handlers,
        IUsersCache usersStore,
        ITelegramChatApi chatApi,
        IOptions<TelegramConfig> config,
        IImageResolver imageResolver,
        ILogger<UpdateHandler> logger)
    {
        _allowedChatIds = config.Value.GetAllowedChatIds();
        _handlers = handlers.ToArray();
        _usersStore = usersStore;
        _chatApi = chatApi;
        _imageResolver = imageResolver;
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
        
        _logger.LogInformation("Handling update from chat {ChatId:}", update.Message?.Chat.Id);

        if (!ChatIsAllowed(update))
        {
            _logger.LogInformation("Chat is not allowed, skipping");
            return;
        }

        if (update.Type != UpdateType.Message)
        {
            _logger.LogDebug("Unsupported update type: {UpdateType}, skipping", update.Type);
            return;
        }

        var message = update.Message;
        if (message is null)
        {
            _logger.LogDebug("Message is null, skipping");
            return;
        }
        
        if (message.Type != MessageType.Text)
        {
            _logger.LogDebug("Unsupported message type: {MessageType}, skipping", message.Type);
            return;
        }
        
        if (message.From is null)
        {
            _logger.LogDebug("Message.From is null, skipping");
            return;
        }
        
        var from = message.From;
        var user = new TelegramUser(from.Id, from.FirstName, from.LastName, from.Username);
        
        // запомним юзера для рендера ответа.
        _usersStore.Set(user);
        
        var text = message.Text ?? string.Empty;
        if (!IsCommand(text))
        {
            _logger.LogDebug("This is not a command, skipping");
            return;
        }

        text = Clean(text);
        
        var command = TelegramCommand.Parse(user, text);
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
                    await using var imageStream = _imageResolver.ResolveById(reply.Image.ImageId);
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

    private bool ChatIsAllowed(Update update)
    {
        return _allowedChatIds.Any(x => x == update.Message?.Chat.Id);
    }

    private static bool IsCommand(string text)
    {
        return text.StartsWith($"{Constants.CommandPrefix}");
    }    
    
    private static string Clean(string text)
    {
        return text.Replace(Constants.CommandPrefix, string.Empty).Trim();
    }

    public Task HandleErrorAsync(ITelegramBotClient botClient, 
        Exception exception, 
        HandleErrorSource source,
        CancellationToken ct)
    {
        _logger.LogError(exception, "Error '{ErrorSource}' occured while handling telegram message", source);
        return Task.CompletedTask;
    }
}