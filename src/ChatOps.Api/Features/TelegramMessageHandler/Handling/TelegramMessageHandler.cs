using ChatOps.Api.Integrations.Telegram.Core;

namespace ChatOps.Api.Features.TelegramMessageHandler.Handling;

internal sealed class TelegramMessageHandler : ITelegramMessageHandler
{
    private readonly ITelegramCommandHandler[] _handlers;
    private readonly ILogger<TelegramMessageHandler> _logger;

    public TelegramMessageHandler(IEnumerable<ITelegramCommandHandler> handlers,
        ILogger<TelegramMessageHandler> logger)
    {
        _handlers = handlers.ToArray();
        _logger = logger;
    }
    
    public async Task<TgHandlerResult> Handle(Message message, CancellationToken ct = default)
    {
        if (message.Type != MessageType.Text)
        {
            _logger.LogInformation("Unsupported message type: {MessageType}", message.Type);
            return new TelegramHandlerFailure("Unsupported message type");
        }
        
        if (message.From is null)
        {
            _logger.LogInformation("Message.From is null");
            return new TelegramHandlerFailure("FROM is null");
        }
        
        var command  = message.Text ?? string.Empty;
        var tokens = CommandTokenCollection.Parse(command);
        if (tokens.Empty)
        {
            _logger.LogInformation("Empty command, skipping");
            return new TelegramReply(string.Empty);
        }
        
        var handler = _handlers.FirstOrDefault(h => h.CanHandle(tokens));
        if (handler is null)
        {
            return new UnknownCommand();
        }
        
        return await handler.Handle(tokens, ct);

        var buffer = tokens.GetBuffer();
        var token = buffer.Take();

        if (token == WellKnownCommandTokens.Env)
        {
            return await HandleEnvCommands(buffer, ct);
        }

        return new UnknownCommand();
    }

    private async Task<TgHandlerResult> HandleEnvCommands(CommandBuffer buffer, CancellationToken ct)
    {
        if (buffer.Empty)
        {
            return new UnknownCommand();
        }

        var token = buffer.Take();
        switch (token)
        {
            case WellKnownCommandTokens.List:
                var response = await _listResourcesUseCase.Execute(ct);
                var list = Stringifier.BuildList(response);
                return new TelegramReply(list);
            
            default:
                return new UnknownCommand();
        }
    }
}

internal interface ITelegramMessageHandler
{
    Task<TgHandlerResult> Handle(Message message, CancellationToken ct = default);
}

internal sealed record TelegramReply(string Text)
{
    public static implicit operator TelegramReply(string text) => new(text);
}
internal sealed record TelegramHandlerFailure(string Error);
internal sealed record UnknownCommand;