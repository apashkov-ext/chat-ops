using ChatOps.App.Core.Models;
using ChatOps.App.SharedPorts;

namespace ChatOps.Infra.SharedAdapters;

internal sealed class InMemoryGetResources : IGetResources
{
    private readonly List<Resource> _resources;

    public InMemoryGetResources(List<Resource> resources)
    {
        _resources = resources;
    }
    
    public async Task<IReadOnlyList<Resource>> Execute(CancellationToken ct = default)
    {
        return _resources.AsReadOnly();
    }
}