using ChatOps.Api.Integrations.Telegram.Core;
using ChatOps.App.Core.Models;
using ChatOps.App.Features.Deploy;
using ChatOps.App.Features.Free;

namespace ChatOps.Api.Features.Deploy;

internal sealed class DeployCommandHandler : ITelegramCommandHandler, ICommandInfo
{
    private readonly IDeployUseCase _deployUseCase;
    public string Command => "deploy <resource> <branch> [ENV=VAL]";
    public string Description => "Установить ветку на ресурс";

    public DeployCommandHandler(IDeployUseCase deployUseCase)
    {
        _deployUseCase = deployUseCase;
    }
    
    public bool CanHandle(TelegramCommand command)
    {
        return command.Tokens.Count is not 0 && command.Tokens[0] == "deploy";
    }

    public async Task<TgHandlerResult> Handle(TelegramCommand command, CancellationToken ct = default)
    {
        var tokens = command.Tokens;
        if (tokens.Count < 3)
        {
            return new TelegramHandlerFailure("Invalid command syntax");
        }
        
        var holder = new HolderId(command.User.Id.ToString());
        var resourceId = new ResourceId(tokens[1]);
        var @ref = new Ref(tokens[2]);
        var variables = GetVariables(command);
        
        var deploy = await _deployUseCase.Execute(holder, resourceId, @ref, variables, ct);
        return await deploy.Match<Task<TgHandlerResult>>(
            success =>
            {
                throw new NotImplementedException();
            },
            resourceNotFound =>
            {
                throw new NotImplementedException();
            },
            inUse =>
            {
                throw new NotImplementedException();
            },
            notReserved =>
            {
                throw new NotImplementedException();
            },
            refNotFound =>
            {
                throw new NotImplementedException();
            },
            failure => Task.FromResult<TgHandlerResult>(new TelegramHandlerFailure(failure.Error))
        );
        
        throw new NotImplementedException();
    }

    private static IEnumerable<Variable> GetVariables(TelegramCommand command)
    {
        var arguments = command.Tokens.Skip(3).ToArray();
        if (arguments.Length == 0)
        {
            yield break;
        }

        foreach (var arg in arguments)
        {
            var parts = arg.Split('=', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (parts.Length != 2)
            {
                continue;
            }
            
            yield return new Variable(parts[0], parts[1]);
        }
    }
}