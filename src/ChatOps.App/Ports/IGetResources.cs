using ChatOps.App.Core.Models;

namespace ChatOps.App.Ports;

public interface IGetResources
{
    Task<IReadOnlyList<Resource>> Execute(CancellationToken ct = default);
}