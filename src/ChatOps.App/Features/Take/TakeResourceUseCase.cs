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
    private readonly IFindResourceById _findResourceById;
    private readonly IUpdateResource _updateResource;

    public TakeResourceUseCase(IFindResourceById findResourceById, 
        IUpdateResource updateResource)
    {
        _findResourceById = findResourceById;
        _updateResource = updateResource;
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
        
        // TODO: если у юзера уже есть занятый ресурс, пусть сперва его освободит.

        if (ResourceReservedByAnotherUser(resource, holder))
        {
            return new TakeResourceInUse(resource.Holder!);
        }
        
        // TODO: если ресурс уже занят этим юзером, ничего не делать и сообщить об этом.
        
        resource.Reserve(holder);
        await _updateResource.Execute(resource, ct);
        
        return new TakeResourceSuccess();
    }
    
    private static bool ResourceReservedByAnotherUser(Resource resource, HolderId currentHolder)
    {
        return 
            resource.State == ResourceState.Reserved 
            && resource.Holder != null && resource.Holder != currentHolder;
    }
}