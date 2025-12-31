namespace ChatOps.Api.Integrations.Telegram.Handling;

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