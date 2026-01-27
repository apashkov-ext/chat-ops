using ChatOps.App.Core.Models;

namespace ChatOps.App.Features.Deploy;

public interface IFindBranch
{
    Task<BranchId?> Execute(BranchId branch, CancellationToken ct = default);
}