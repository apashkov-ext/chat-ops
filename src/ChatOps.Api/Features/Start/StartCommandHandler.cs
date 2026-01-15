using ChatOps.Api.Integrations;
using ChatOps.Api.Integrations.Telegram.Core;

namespace ChatOps.Api.Features.Start;

internal sealed class StartCommandHandler : ITelegramCommandHandler, ICommandInfo
{
    public string Command => "start";
    public string Description => "Поздороваться";

    public bool CanHandle(TelegramCommand command)
    {
        return command.Tokens is ["start"];
    }

    public async Task<TgHandlerResult> Handle(TelegramCommand command, CancellationToken ct = default)
    {
        const string message = $"""
                                       👋 Привет. 
                                       Меня зовут Антонио, я - ChatOps.
                                       Давай накатывать вместе!
                                       
                                       Чтобы узнать, что я умею, напиши <code>{Constants.CommandPrefix} help</code>
                                       """;
        
        var text = new TelegramText(message);
        return new TelegramReply(text);
    }
}