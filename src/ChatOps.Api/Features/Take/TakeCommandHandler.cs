using ChatOps.Api.Integrations.Telegram.Core;
using ChatOps.App.Core.Models;
using ChatOps.App.Features.Take;

namespace ChatOps.Api.Features.Take;

internal sealed class TakeCommandHandler : ITelegramCommandHandler, ICommandInfo
{
    private readonly ITakeResourceUseCase _takeResource;
    private readonly ITakeResourceAndDeployUseCase _takeResourceAndDeploy;

    public TakeCommandHandler(ITakeResourceUseCase takeResource,
        ITakeResourceAndDeployUseCase takeResourceAndDeploy)
    {
        _takeResource = takeResource;
        _takeResourceAndDeploy = takeResourceAndDeploy;
    }

    public string Command => "take <resource>";
    public string Description => "Занять указанный ресурс";

    public bool CanHandle(TelegramCommand command)
    {
        return command.Tokens.Count is not 0 && command.Tokens[0] == "take";
    }

    public async Task<TgHandlerResult> Handle(TelegramCommand command, CancellationToken ct = default)
    {
        var tokens = command.Tokens;
        if (tokens.Count == 1)
        {
            return new TelegramHandlerFailure("Invalid command syntax");
        }
        
        var holder = new HolderId(command.User.Id.ToString());
        var resourceName = command.Tokens[1];

        if (tokens.Count == 2)
        {
            var takeResource = await _takeResource.Execute(holder, resourceName, ct);
            return await takeResource.Match<Task<TgHandlerResult>>(
                success => Task.FromResult<TgHandlerResult>(new TelegramReply(success.Reply)), 
                failure => Task.FromResult<TgHandlerResult>(new TelegramHandlerFailure(failure.Error))
            );
        }

        var branch = tokens[2];

        if (tokens.Count == 3)
        {
            var takeAndDeploy = await _takeResourceAndDeploy.Execute(holder, resourceName, branch, ct);
            return await takeAndDeploy.Match<Task<TgHandlerResult>>(
                success => Task.FromResult<TgHandlerResult>(new TelegramReply(success.Reply)), 
                failure => Task.FromResult<TgHandlerResult>(new TelegramHandlerFailure(failure.Error))
            );
        }
        
        return new TelegramHandlerFailure("Invalid command syntax");
    }
}