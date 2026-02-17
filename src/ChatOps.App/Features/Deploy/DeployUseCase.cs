using ChatOps.App.Core.Models;
using ChatOps.App.SharedPorts;

namespace ChatOps.App.Features.Deploy;

public interface IDeployUseCase
{
    Task<DeployResult> Execute(HolderId holderId,
        ResourceId resourceId,
        RefName @ref,
        IEnumerable<Variable> variables,
        CancellationToken ct = default);
}

internal sealed class DeployUseCase : IDeployUseCase
{
    private readonly IFindResourceById _findResourceById;
    private readonly IFindRef _findRef;
    private readonly ICreatePipeline _createPipeline;

    public DeployUseCase(
        IFindResourceById findResourceById,
        IFindRef findRef,
        ICreatePipeline createPipeline)
    {
        _findResourceById = findResourceById;
        _findRef = findRef;
        _createPipeline = createPipeline;
    }
    
    public async Task<DeployResult> Execute(HolderId holderId,
        ResourceId resourceId, 
        RefName refName, 
        IEnumerable<Variable> variables,
        CancellationToken ct = default)
    {
        var resource = await _findResourceById.Execute(resourceId, ct);
        if (resource is null)
        {
            return new DeployResourceNotFound();
        }

        if (!ResourceReservedByCurrentUser(resource, holderId))
        {
            return new DeployResourceNotReserved();
        }
        
        var findRef = await _findRef.Execute(refName, ct);
        return await findRef.Match<Task<DeployResult>>(
            async success => await CreatePipeline(resource, success.Ref, variables, ct),
            _ =>
            {
                var nf = new DeployRefNotFound();
                return Task.FromResult<DeployResult>(nf);
            },
            failure =>
            {
                var reason = MapReason(failure.Reason);
                return Task.FromResult<DeployResult>(new DeployFailure(reason));
            }
        );
    }

    private async Task<DeployResult> CreatePipeline(
        Resource resource,
        Ref @ref,
        IEnumerable<Variable> variables,
        CancellationToken ct)
    {
        var createPipeline = await _createPipeline.Execute(resource, @ref, variables, ct);
        return await createPipeline.Match<Task<DeployResult>>(
            success =>
            {
                var res = new DeploySuccess(success.Pipeline);
                return Task.FromResult<DeployResult>(res);
            },
            failure =>
            {
                var reason = MapReason(failure.Reason);
                return Task.FromResult<DeployResult>(new DeployFailure(reason));
            }
        );
    }

    private static DeployFailureReason MapReason(FindRefFailureReason reason)
    {
        switch (reason)
        {
            case FindRefFailureReason.Permanent:
                return DeployFailureReason.IncorrectIntegration;
            case FindRefFailureReason.Transient:
                return DeployFailureReason.PleaseTryAgain;
            case FindRefFailureReason.Unknown:
            default:
                return DeployFailureReason.Unknown;
        }
    }    
    
    private static DeployFailureReason MapReason(CreatePipelineFailureReason reason)
    {
        switch (reason)
        {
            case CreatePipelineFailureReason.Permanent:
                return DeployFailureReason.IncorrectIntegration;
            case CreatePipelineFailureReason.Transient:
                return DeployFailureReason.PleaseTryAgain;
            case CreatePipelineFailureReason.Unknown:
            default:
                return DeployFailureReason.Unknown;
        }
    }

    private static bool ResourceReservedByCurrentUser(Resource resource, HolderId currentHolder)
    {
        return resource.State == ResourceState.Reserved && resource.Holder == currentHolder;
    }
}