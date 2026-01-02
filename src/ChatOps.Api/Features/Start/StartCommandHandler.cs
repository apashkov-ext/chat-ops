using ChatOps.Api.Features.TelegramMessageHandler.Handling;
using ChatOps.Api.Integrations.Telegram.Core;

namespace ChatOps.Api.Features.Start;

internal sealed class StartCommandHandler : ITelegramCommandHandler
{
    public bool CanHandle(CommandTokenCollection collection)
    {
        return collection.Tokens is ["/start"];
    }

    public async Task<TgHandlerResult> Handle(CommandTokenCollection collection, CancellationToken ct = default)
    {
        return new TelegramReply("Будем знакомы");
    }
}