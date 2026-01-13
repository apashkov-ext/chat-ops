namespace ChatOps.Api.Integrations.Telegram.Core;

internal interface ITelegramCommandHandler
{
    bool CanHandle(TelegramCommand collection);
    Task<TgHandlerResult> Handle(TelegramCommand collection, CancellationToken ct = default);
}

internal interface ICommandInfo
{
    string Command { get; }
    string Description { get; }
}

internal sealed record TelegramReply(string Text)
{
    public static implicit operator TelegramReply(string text) => new(text);
}
internal sealed record TelegramHandlerFailure(string Error);
internal sealed record UnknownCommand;