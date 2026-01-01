using ChatOps.Api.Features.TelegramMessageHandler.Handling;
using ChatOps.Api.Integrations.Telegram.Core;

namespace ChatOps.Api.Features.Help;

internal sealed class HelpCommandHandler : ITelegramCommandHandler
{
    public bool CanHandle(CommandTokenCollection tokens) => tokens.ToString() == WellKnownCommandTokens.Help;

    public async Task<TgHandlerResult> Handle(CommandTokenCollection tokens, CancellationToken ct = default)
    {
        var help = Stringifier.BuildHelpText();
        return new TelegramReply(help);

    }
}