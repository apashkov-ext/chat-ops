using ChatOps.App.Core.Models;
using ChatOps.App.SharedPorts;

namespace ChatOps.App.Features.Free;

public interface IFreeResourceUseCase
{
    Task<FreeResourceResult> Execute(
        HolderId holder,
        ResourceId resourceId,
        CancellationToken ct = default);
}

internal sealed class FreeResourceUseCase : IFreeResourceUseCase
{
    private readonly IFindResourceById _findResourceById;
    private readonly IUpdateResource _updateResource;

    public FreeResourceUseCase(IFindResourceById findResourceById, 
        IUpdateResource updateResource)
    {
        _findResourceById = findResourceById;
        _updateResource = updateResource;
    }
    
    public async Task<FreeResourceResult> Execute(
        HolderId holder,
        ResourceId resourceId,
        CancellationToken ct = default)
    {
        var resource = await _findResourceById.Execute(resourceId, ct);
        if (resource is null)
        {
            return new FreeResourceNotFound();
        }
        
        if (ResourceReservedByAnotherUser(resource, holder))
        {
            return new FreeResourceInUse(resource.Holder!);
        }
        
        // TODO: если ресурс уже свободен, ничего не делать и сообщить об этом.
        
        resource.Free();
        await _updateResource.Execute(resource, ct);
        
        return new FreeResourceSuccess();
    }

    private static bool ResourceReservedByAnotherUser(Resource resource, HolderId currentHolder)
    {
        return 
            resource.State == ResourceState.Reserved 
            && resource.Holder != null && resource.Holder != currentHolder;
    }
}