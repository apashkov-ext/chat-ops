using ChatOps.Api.Integrations.Telegram.Core;
using ChatOps.App.Core.Models;
using ChatOps.App.Features.Free;

namespace ChatOps.Api.Features.Free;

internal sealed class FreeCommandHandler : ITelegramCommandHandler, ICommandInfo
{
    private readonly IFreeResourceUseCase _freeResourceUseCase;
    public string Command => "free <resource>";
    public string Description => "Освободить указанный ресурс";

    public FreeCommandHandler(IFreeResourceUseCase freeResourceUseCase)
    {
        _freeResourceUseCase = freeResourceUseCase;
    }

    public bool CanHandle(TelegramCommand command)
    {
        return command.Tokens.Count is not 0 && command.Tokens[0] == "free";
    }

    public async Task<TgHandlerResult> Handle(TelegramCommand command, CancellationToken ct = default)
    {
        var tokens = command.Tokens;
        if (tokens.Count != 2)
        {
            return new TelegramHandlerFailure("Invalid command syntax");
        }
        
        var holderId = new HolderId(command.User.Id.ToString());
        var resourceId = new ResourceId(tokens[1]);
        
        var freeResource = await _freeResourceUseCase.Execute(holderId, resourceId, ct);
        return await freeResource.Match<Task<TgHandlerResult>>(
            success => Task.FromResult<TgHandlerResult>(new TelegramReply()), 
            failure => Task.FromResult<TgHandlerResult>(new TelegramHandlerFailure(failure.Error))
        );
    }
}