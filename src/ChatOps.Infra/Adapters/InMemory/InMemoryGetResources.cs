using ChatOps.App.Core.Models;
using ChatOps.App.Ports;

namespace ChatOps.Infra.Adapters.InMemory;

internal sealed class InMemoryGetResources : IGetResources
{
    public Task<IReadOnlyList<Resource>> Execute(CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}