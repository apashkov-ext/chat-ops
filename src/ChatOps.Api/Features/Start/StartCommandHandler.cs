using ChatOps.Api.Integrations;
using ChatOps.Api.Integrations.Telegram;
using ChatOps.Api.Integrations.Telegram.Core;

namespace ChatOps.Api.Features.Start;

internal sealed class StartCommandHandler : ITelegramCommandHandler, ICommandInfo
{
    private readonly IApplicationVersionResolver _versionResolver;
    public string Command => "start";
    public string Description => "Поздороваться";

    public StartCommandHandler(IApplicationVersionResolver versionResolver)
    {
        _versionResolver = versionResolver;
    }

    public bool CanHandle(TelegramCommand command)
    {
        return command.Tokens is ["start"];
    }

    public Task<TgHandlerResult> Handle(TelegramCommand command, CancellationToken ct = default)
    {
        var version = _versionResolver.GetVersion();
        var message = $"""
                                       👋 Привет. 
                                       Меня зовут Антонио, я - ChatOps.
                                       Давай накатывать вместе!
                                       
                                       Чтобы узнать, что я умею, напиши <code>{Constants.CommandPrefix} help</code>
                                       v.{TgHtml.Code(version)}
                                       """;
        
        var text = new TelegramText(message);
        return Task.FromResult<TgHandlerResult>(new TelegramReply(text));
    }
}