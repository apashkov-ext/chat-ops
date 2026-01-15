using ChatOps.App.Core.Models;
using ChatOps.App.SharedPorts;

namespace ChatOps.Infra.SharedAdapters;

internal sealed class InMemoryFindResourceById : IFindResourceById
{
    private readonly List<Resource> _resources;

    public InMemoryFindResourceById(List<Resource> resources)
    {
        _resources = resources;
    }
    
    public async Task<Resource?> Execute(ResourceId id, CancellationToken ct = default)
    {
        return _resources.Find(x => x.Id == id);
    }
}