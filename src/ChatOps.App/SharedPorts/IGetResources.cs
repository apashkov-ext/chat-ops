using ChatOps.App.Core.Models;

namespace ChatOps.App.SharedPorts;

public interface IGetResources
{
    Task<IReadOnlyList<Resource>> Execute(CancellationToken ct = default);
}