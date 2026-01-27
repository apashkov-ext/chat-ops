using ChatOps.Api.Integrations.Telegram.Core;
using ChatOps.App.Features.Deploy;
using ChatOps.App.Features.Free;

namespace ChatOps.Api.Features.Deploy;

internal sealed class DeployCommandHandler : ITelegramCommandHandler, ICommandInfo
{
    private readonly IDeployUseCase _deployUseCase;
    public string Command => "deploy <resource> <branch>";
    public string Description => "Установить ветку на ресурс";

    public DeployCommandHandler(IDeployUseCase deployUseCase)
    {
        _deployUseCase = deployUseCase;
    }
    
    public bool CanHandle(TelegramCommand collection)
    {
        throw new NotImplementedException();
    }

    public Task<TgHandlerResult> Handle(TelegramCommand collection, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}