using Telegram.Bot.Types;

namespace ChatOps.Api.Integrations.Telegram.Handling;

internal interface ITelegramMessageHandler
{
    Task<HandleTelegramMessageResult> Handle(Message message, CancellationToken ct = default);
}

internal sealed record HandleTelegramMessageResult(
    HandleTelegramMessageCode Code, 
    string? Result, 
    string? Error )
{
    public bool HasResult => !string.IsNullOrWhiteSpace(Result);
    
    public static HandleTelegramMessageResult UnknownCommand(string error)
    {
        return new HandleTelegramMessageResult(HandleTelegramMessageCode.UnknownCommand, default, error);
    }
    
    public static HandleTelegramMessageResult Success(string result)
    {
        return new HandleTelegramMessageResult(HandleTelegramMessageCode.Success, result, null);
    }

    public static HandleTelegramMessageResult Failure(string error)
    {
        return new HandleTelegramMessageResult(HandleTelegramMessageCode.Failure, default, error);
    }
}

internal enum HandleTelegramMessageCode
{
    UnknownCommand,
    Success,
    Failure
}