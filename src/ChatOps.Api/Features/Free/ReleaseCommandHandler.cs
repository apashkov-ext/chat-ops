using ChatOps.Api.Integrations.Telegram.Core;

namespace ChatOps.Api.Features.Free;

internal sealed class FreeCommandHandler : ITelegramCommandHandler, ICommandInfo
{
    public string Command => "free <resource>";
    public string Description => "Освободить указанный ресурс";

    public bool CanHandle(TelegramCommand command)
    {
        return command.Tokens.Count is not 0 && command.Tokens[0] == "free";
    }

    public Task<TgHandlerResult> Handle(TelegramCommand command, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}