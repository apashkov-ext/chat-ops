using ChatOps.App.Core.Models;

namespace ChatOps.App.Features.Deploy;

public interface IDeployUseCase
{
    Task<DeployResult> Execute(HolderId holderId,
        ResourceId resourceId, 
        BranchId branch, 
        CancellationToken ct = default);
}

internal sealed class DeployUseCase : IDeployUseCase
{
    public Task<DeployResult> Execute(HolderId holderId,
        ResourceId resourceId, 
        BranchId branch, 
        CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}