using ChatOps.App.Core.Models;
using ChatOps.App.Ports;

namespace ChatOps.Infra.Adapters.InMemory;

internal sealed class InMemoryGetResources : IGetResources
{
    private readonly List<Resource> _resources =
    [
        new()
        {
            Id = new ResourceId(Guid.NewGuid().ToString()),
            Name = "dev",
            State = ResourceState.Free
        },
        new()
        {
            Id = new ResourceId(Guid.NewGuid().ToString()),
            Name = "dev1",
            State = ResourceState.Reserved,
            Holder = "@user"
        },
        new()
        {
            Id = new ResourceId(Guid.NewGuid().ToString()),
            Name = "dev2",
            State = ResourceState.Free
        }
    ];
    
    public async Task<IReadOnlyList<Resource>> Execute(CancellationToken ct = default)
    {
        return _resources.AsReadOnly();
    }
}