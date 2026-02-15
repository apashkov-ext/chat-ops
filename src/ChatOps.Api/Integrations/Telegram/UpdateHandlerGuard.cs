using Microsoft.Extensions.Options;

namespace ChatOps.Api.Integrations.Telegram;

internal interface IUpdateHandlerGuard
{
    bool CanHandle(Update update);
}

internal sealed class UpdateHandlerGuard : IUpdateHandlerGuard
{
    private readonly long[] _allowedChatIds;
    private readonly ILogger<UpdateHandlerGuard> _logger;

    public UpdateHandlerGuard(
        IOptions<TelegramConfig> config,
        ILogger<UpdateHandlerGuard> logger)
    {
        _allowedChatIds = config.Value.GetAllowedChatIds();
        _logger = logger;
    }
    
    public bool CanHandle(Update update)
    {
        var message = update.Message;
        if (message is null)
        {
            _logger.LogDebug("Message is null, skipping");
            return false;
        }

        if (!ChatIsAllowed(update))
        {
            _logger.LogDebug("Chat is not allowed, skipping");
            return false;
        }

        if (update.Type != UpdateType.Message)
        {
            _logger.LogDebug("Unsupported update type: {UpdateType}, skipping", update.Type);
            return false;
        }
        
        if (message.Type != MessageType.Text)
        {
            _logger.LogDebug("Unsupported message type: {MessageType}, skipping", message.Type);
            return false;
        }
        
        return true;
    }
    
    private bool ChatIsAllowed(Update update)
    {
        return _allowedChatIds.Any(x => x == update.Message?.Chat.Id);
    }
}