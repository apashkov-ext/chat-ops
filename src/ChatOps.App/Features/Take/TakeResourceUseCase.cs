using ChatOps.App.Core.Models;
using ChatOps.App.SharedPorts;

namespace ChatOps.App.Features.Take;

public interface ITakeResourceUseCase
{
    Task<TakeResourceResult> Execute(
        HolderId holder,
        ResourceId resourceId, 
        CancellationToken ct = default);
}

public sealed class TakeResourceUseCase : ITakeResourceUseCase
{
    public const int MaxResourcesPerUser = 1;
    private readonly IFindResourceById _findResourceById;
    private readonly IUpdateResource _updateResource;
    private readonly ICountReservedResourcesByHolder _countReservedResourcesByHolder;

    public TakeResourceUseCase(
        IFindResourceById findResourceById, 
        IUpdateResource updateResource,
        ICountReservedResourcesByHolder countReservedResourcesByHolder)
    {
        _findResourceById = findResourceById;
        _updateResource = updateResource;
        _countReservedResourcesByHolder = countReservedResourcesByHolder;
    }
    
    public async Task<TakeResourceResult> Execute(
        HolderId holder, 
        ResourceId resourceId,
        CancellationToken ct = default)
    {
        var resource = await _findResourceById.Execute(resourceId, ct);
        if (resource is null)
        {
            return new TakeResourceNotFound();
        }

        if (resource.State == ResourceState.Reserved)
        {
            if (resource.Holder == holder)
            {
                return new TakeResourceAlreadyReserved();
            }

            if (resource.Holder != null)
            {
                return new TakeResourceInUse(resource.Holder);
            }
        }
        
        var count = await _countReservedResourcesByHolder.Execute(holder, ct);
        if (count > MaxResourcesPerUser)
        {
            return new TakeResourceLimitExceeded();
        }
            
        resource.Reserve(holder);
        await _updateResource.Execute(resource, ct);
        
        return new TakeResourceSuccess();
    }
}