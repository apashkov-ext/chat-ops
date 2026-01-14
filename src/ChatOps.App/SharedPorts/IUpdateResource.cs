using ChatOps.App.Core.Models;

namespace ChatOps.App.SharedPorts;

public interface IUpdateResource
{
    Task Execute(Resource resource, CancellationToken ct = default);
}