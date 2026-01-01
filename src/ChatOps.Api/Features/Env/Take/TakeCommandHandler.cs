using ChatOps.Api.Features.TelegramMessageHandler.Handling;
using ChatOps.Api.Integrations.Telegram.Core;

namespace ChatOps.Api.Features.Env.Take;

internal sealed class TakeCommandHandler : ITelegramCommandHandler
{
    public bool CanHandle(CommandTokenCollection tokens)
    {
        throw new NotImplementedException();
    }

    public Task<TgHandlerResult> Handle(CommandTokenCollection tokens, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}