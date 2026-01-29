using ChatOps.App.Core.Models;

namespace ChatOps.App.Features.Deploy;

public interface IFindRef
{
    Task<Ref?> Execute(Ref @ref, CancellationToken ct = default);
}