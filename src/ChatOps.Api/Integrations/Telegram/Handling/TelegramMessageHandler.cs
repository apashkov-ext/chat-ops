using ChatOps.App.UseCases.ListResources;
using ChatOps.App.UseCases.ReleaseResource;
using ChatOps.App.UseCases.TakeResource;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

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
    
    public async Task<HandleTelegramMessageResult> Handle(Message message, CancellationToken ct = default)
    {
        if (message.Type != MessageType.Text)
        {
            _logger.LogInformation("Unsupported message type: {MessageType}", message.Type);
            return HandleTelegramMessageResult.Failure("Unsupported message type");
        }
        
        if (message.From is null)
        {
            _logger.LogInformation("Message.From is null");
            return HandleTelegramMessageResult.Failure("FROM is null");
        }
        
        var command  = message.Text ?? string.Empty;
        var tokens = CommandTokenCollection.Parse(command);
        if (tokens.Empty)
        {
            _logger.LogInformation("Empty command, skipping");
            return HandleTelegramMessageResult.Success("Введите /help для отображения доступных команд");
        }

        var buffer = tokens.GetBuffer();
        var token = buffer.Take();

        if (token == WellKnownCommandTokens.Start)
        {
            return HandleTelegramMessageResult.Success("Будем знакомы");
        }

        if (token == WellKnownCommandTokens.Help)
        {
            var help = Stringifier.BuildHelpText();
            return HandleTelegramMessageResult.Success(help);
        }

        if (token == WellKnownCommandTokens.Env)
        {
            return await HandleEnvCommands(buffer);
        }

        return  HandleTelegramMessageResult.UnknownCommand();
    }

    private async Task<HandleTelegramMessageResult> HandleEnvCommands(CommandBuffer buffer)
    {
        if (buffer.Empty)
        {
            return HandleTelegramMessageResult.UnknownCommand();
        }

        var token = buffer.Take();
        switch (token)
        {
            case WellKnownCommandTokens.List:
                var response = await _listResourcesUseCase.Execute();
                var list = Stringifier.BuildList(response);
                return HandleTelegramMessageResult.Success(list);
            
            default:
                return HandleTelegramMessageResult.UnknownCommand();
        }
    }
}