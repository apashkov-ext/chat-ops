using ChatOps.App.Core.Models;
using ChatOps.App.SharedPorts;

namespace ChatOps.App.Features.Take;

public interface ITakeResourceUseCase
{
    Task<TakeResourceResult> Execute(
        HolderId holderId,
        ResourceId resourceId, 
        CancellationToken ct = default);
}

public sealed class TakeResourceUseCase : ITakeResourceUseCase
{
    private readonly IFindResourceById _findResourceById;
    private readonly IUpdateResource _updateResource;

    public TakeResourceUseCase(IFindResourceById findResourceById, IUpdateResource updateResource)
    {
        _findResourceById = findResourceById;
        _updateResource = updateResource;
    }
    
    public async Task<TakeResourceResult> Execute(
        HolderId holderId, 
        ResourceId resourceId,
        CancellationToken ct = default)
    {
        var resource = await _findResourceById.Execute(resourceId, ct);
        if (resource is null)
        {
            return new TakeResourceNotFound();
        }

        if (resource.State == ResourceState.Reserved 
            && resource.Holder != null && resource.Holder != holderId)
        {
            return new TakeResourceAlreadyReserved(resource.Holder);
        }
        
        resource.Reserve(holderId);
        await _updateResource.Execute(resource, ct);
        
        return new TakeResourceSuccess();
    }
}