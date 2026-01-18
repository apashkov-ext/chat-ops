using ChatOps.App.Core.Models;

namespace ChatOps.App.Features.Deploy;

public interface IFindBranch
{
    Task<Branch?> Execute(Branch branch, CancellationToken ct = default);
}