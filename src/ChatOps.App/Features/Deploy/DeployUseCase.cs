using ChatOps.App.Core.Models;
using ChatOps.App.SharedPorts;

namespace ChatOps.App.Features.Deploy;

public interface IDeployUseCase
{
    Task<DeployResult> Execute(HolderId holderId,
        ResourceId resourceId, 
        Branch branch, 
        CancellationToken ct = default);
}

internal sealed class DeployUseCase : IDeployUseCase
{
    private readonly IFindResourceById _findResourceById;
    private readonly IFindBranch _findBranch;

    public DeployUseCase(
        IFindResourceById findResourceById,
        IFindBranch findBranch)
    {
        _findResourceById = findResourceById;
        _findBranch = findBranch;
    }
    
    public async Task<DeployResult> Execute(HolderId holderId,
        ResourceId resourceId, 
        Branch branch, 
        CancellationToken ct = default)
    {
        var resource = await _findResourceById.Execute(resourceId, ct);
        if (resource is null)
        {
            return new DeployResourceNotFound();
        }

        if (ResourceReservedByAnotherUser(resource, holderId))
        {
            return new DeployResourceInUse(holderId);
        }
        
        // проверить, существует ли ветка
        
        // проверить, запущен ли уже пайплайн с такими же параметрами (если это возможно). Если запущен - что тогда? Варианты:
        // - остановить его и запустить новый
        // - запустить новый, не останавливая старый (поставить в очередь).
        // - сообщить юзеру и ничего не делать.
        // TODO: сколько максимум pipeline можно запускать на одном ресурсе?

        throw new NotImplementedException();
    }
    
    private static bool ResourceReservedByAnotherUser(Resource resource, HolderId currentHolder)
    {
        return 
            resource.State == ResourceState.Reserved 
            && resource.Holder != null && resource.Holder != currentHolder;
    }
}