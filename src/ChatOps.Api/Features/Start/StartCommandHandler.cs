using ChatOps.Api.Features.TelegramMessageHandler.Handling;
using ChatOps.Api.Integrations.Telegram.Core;

namespace ChatOps.Api.Features.Start;

internal sealed class StartCommandHandler : ITelegramCommandHandler
{
    public bool CanHandle(CommandTokenCollection tokens) => tokens.ToString() == WellKnownCommandTokens.Start;

    public async Task<TgHandlerResult> Handle(CommandTokenCollection tokens, CancellationToken ct = default)
    {
        return new TelegramReply("Будем знакомы");
    }
}