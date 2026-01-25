using ChatOps.App.Core.Models;
using ChatOps.App.Features.Take;

namespace ChatOps.Infra.Features.Take;

internal sealed class InMemoryCountReservedResourcesByHolder : ICountReservedResourcesByHolder
{
    private readonly List<Resource> _resources;

    public InMemoryCountReservedResourcesByHolder(List<Resource> resources)
    {
        _resources = resources;
    }
    
    public async Task<int> Execute(HolderId holder, CancellationToken ct = default)
    {
        return _resources.Count(x => x.State == ResourceState.Reserved && x.Holder == holder);
    }
}