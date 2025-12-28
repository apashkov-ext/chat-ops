using ChatOps.App.Core.Models;

namespace ChatOps.App.Ports;

public interface IFindResourceByName
{
    Task<Resource?> Execute(string name, CancellationToken ct = default);
}