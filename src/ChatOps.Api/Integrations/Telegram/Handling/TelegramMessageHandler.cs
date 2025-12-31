using ChatOps.App.UseCases.ListResources;
using ChatOps.App.UseCases.ReleaseResource;
using ChatOps.App.UseCases.TakeResource;
using OneOf;

namespace ChatOps.Api.Integrations.Telegram.Handling;

internal sealed class TelegramMessageHandler : ITelegramMessageHandler
{
    private readonly IListResourcesUseCase _listResourcesUseCase;
    private readonly ITakeResourceUseCase _takeResourceUseCase;
    private readonly IReleaseResourceUseCase _releaseResourceUseCase;
    private readonly ILogger<TelegramMessageHandler> _logger;

    public TelegramMessageHandler(
        IListResourcesUseCase listResourcesUseCase,
        ITakeResourceUseCase takeResourceUseCase,
        IReleaseResourceUseCase releaseResourceUseCase,
        ILogger<TelegramMessageHandler> logger)
    {
        _listResourcesUseCase = listResourcesUseCase;
        _takeResourceUseCase = takeResourceUseCase;
        _releaseResourceUseCase = releaseResourceUseCase;
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
            return new TelegramReply($"Введите {WellKnownCommandTokens.Help} для отображения доступных команд");
        }

        var buffer = tokens.GetBuffer();
        var token = buffer.Take();

        if (token == WellKnownCommandTokens.Start)
        {
            return new TelegramReply("Будем знакомы");
        }

        if (token == WellKnownCommandTokens.Help)
        {
            var help = Stringifier.BuildHelpText();
            return new TelegramReply(help);
        }

        if (token == WellKnownCommandTokens.Env)
        {
            return await HandleEnvCommands(buffer);
        }

        return new UnknownCommand();
    }

    private async Task<TgHandlerResult> HandleEnvCommands(CommandBuffer buffer)
    {
        if (buffer.Empty)
        {
            return new UnknownCommand();
        }

        var token = buffer.Take();
        switch (token)
        {
            case WellKnownCommandTokens.List:
                var response = await _listResourcesUseCase.Execute();
                var list = Stringifier.BuildList(response);
                return new TelegramReply(list);
            
            default:
                return new UnknownCommand();
        }
    }
}