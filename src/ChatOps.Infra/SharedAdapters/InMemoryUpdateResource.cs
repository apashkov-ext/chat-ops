using ChatOps.App.Core.Models;
using ChatOps.App.SharedPorts;

namespace ChatOps.Infra.SharedAdapters;

internal sealed class InMemoryUpdateResource : IUpdateResource
{
    private readonly List<Resource> _resources;

    public InMemoryUpdateResource(List<Resource> resources)
    {
        _resources = resources;
    }
    
    public async Task Execute(Resource resource, CancellationToken ct = default)
    {
        var index = _resources.FindIndex(x => x.Id == resource.Id);
        if (index == -1)
        {
            throw new Exception($"Resource {resource.Id} not found");
        }

        _resources[index] = resource;
    }
}