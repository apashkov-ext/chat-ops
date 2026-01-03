namespace ChatOps.Api.Integrations.Telegram.Core;

internal interface ITelegramCommandHandler
{
    bool CanHandle(CommandTokenCollection collection);
    Task<TgHandlerResult> Handle(CommandTokenCollection collection, CancellationToken ct = default);
}

internal sealed record TelegramReply(string Text)
{
    public static implicit operator TelegramReply(string text) => new(text);
}
internal sealed record TelegramHandlerFailure(string Error);
internal sealed record UnknownCommand;
internal sealed record MissingRequiredArgument(string ArgumentPattern);
