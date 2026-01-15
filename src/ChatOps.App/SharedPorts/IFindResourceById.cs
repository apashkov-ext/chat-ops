using ChatOps.App.Core.Models;

namespace ChatOps.App.SharedPorts;

public interface IFindResourceById
{
    Task<Resource?> Execute(ResourceId id, CancellationToken ct = default);
}