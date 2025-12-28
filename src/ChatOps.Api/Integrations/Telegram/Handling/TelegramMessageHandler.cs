using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ChatOps.Api.Integrations.Telegram.Handling;

internal sealed class TelegramMessageHandler : ITelegramMessageHandler
{
    private readonly ILogger<TelegramMessageHandler> _logger;

    public TelegramMessageHandler(ILogger<TelegramMessageHandler> logger)
    {
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
            return HandleTelegramMessageResult.Success(string.Empty);
        }
        
        var queue = new Queue<string>(tokens.Tokens);
        var token = queue.Dequeue();

        if (token == WellKnownCommandTokens.Start)
        {
            return HandleTelegramMessageResult.Success("Будем знакомы");
        }

        if (token != WellKnownCommandTokens.Env)
        {
            return HandleTelegramMessageResult.UnknownCommand(token);
        }

        if (queue.Count == 0)
        {
            var help = BuildHelpText();
            return HandleTelegramMessageResult.Success(help);
        }

        return await HandleEnvCommands(queue);
    }

    private static string BuildHelpText()
    {
        return
            $"""
            Доступные команды:
            {WellKnownCommandTokens.Env} {WellKnownCommandTokens.List}
            {WellKnownCommandTokens.Env} {WellKnownCommandTokens.Take} dev1 [my-branch]
            {WellKnownCommandTokens.Env} {WellKnownCommandTokens.Release} dev1
            """
            ;
    }

    private async Task<HandleTelegramMessageResult> HandleEnvCommands(Queue<string> restTokens)
    {
        return HandleTelegramMessageResult.UnknownCommand(string.Empty);
    }
}