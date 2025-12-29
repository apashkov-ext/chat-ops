using Telegram.Bot.Types;

namespace ChatOps.Api.Integrations.Telegram.Handling;

internal interface ITelegramMessageHandler
{
    Task<HandleTelegramMessageResult> Handle(Message message, CancellationToken ct = default);
}

internal sealed record HandleTelegramMessageResult(
    HandleTelegramMessageCode Code, 
    string? Response)
{
    public bool HasResponse => !string.IsNullOrWhiteSpace(Response);
    
    public static HandleTelegramMessageResult UnknownCommand()
    {
        return new HandleTelegramMessageResult(HandleTelegramMessageCode.UnknownCommand, null);
    }
    
    public static HandleTelegramMessageResult Success(string response)
    {
        return new HandleTelegramMessageResult(HandleTelegramMessageCode.Success, response);
    }

    public static HandleTelegramMessageResult Failure(string error)
    {
        return new HandleTelegramMessageResult(HandleTelegramMessageCode.Failure, error);
    }
}

internal enum HandleTelegramMessageCode
{
    UnknownCommand,
    Success,
    Failure
}