using ChatOps.Api.Integrations.Telegram.Core;

namespace ChatOps.Api.Features.Take;

internal sealed class TakeCommandHandler : ITelegramCommandHandler
{
    public bool CanHandle(CommandTokenCollection collection)
    {
        return collection.Tokens.Count is 2 or 3 && collection.Tokens[0] == "take";
    }

    public async Task<TgHandlerResult> Handle(CommandTokenCollection tokens, CancellationToken ct = default)
    {
        return new TelegramHandlerFailure("Invalid command syntax");
    }
}