namespace ChatOps.Api.Integrations.Telegram.Core;

internal interface ITelegramCommandHandler
{
    bool CanHandle(TelegramCommand command);
    Task<TgHandlerResult> Handle(TelegramCommand command, CancellationToken ct = default);
}

internal interface ICommandInfo
{
    string Command { get; }
    string Description { get; }
}

internal record TelegramReply
{
    public TelegramImage? Image { get; }
    public TelegramText? Text { get; }

    public TelegramReply() { }

    public TelegramReply(TelegramText text)
    {
        Text = text;
    }    
    
    public TelegramReply(TelegramImage image)
    {
        Image = image;
    }    
    
    public TelegramReply(TelegramText text, TelegramImage image)
    {
        Text = text;
        Image = image;
    }
}
internal sealed record TelegramHandlerFailure(string Error);
internal sealed record UnknownCommand;

internal sealed record TelegramText(string Text);
internal sealed record TelegramImage(string ImageId);