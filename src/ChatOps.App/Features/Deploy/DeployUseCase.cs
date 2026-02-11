using ChatOps.App.Core.Models;
using ChatOps.App.SharedPorts;

namespace ChatOps.App.Features.Deploy;

public interface IDeployUseCase
{
    Task<DeployResult> Execute(HolderId holderId,
        ResourceId resourceId, 
        Ref @ref, 
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
        Ref @ref, 
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
        
        var r = await _findRef.Execute(@ref, ct);
        if (r is null)
        {
            return new DeployRefNotFound();
        }
        
        var createPipeline = await _createPipeline.Execute(resource, @ref, variables, ct);
        return await createPipeline.Match<Task<DeployResult>>(
            success =>
            {
                var res = new DeploySuccess(success.Pipeline);
                return Task.FromResult<DeployResult>(res);
            },
            alreadyExists =>
            {
                var res = new DeployInProcess(alreadyExists.Pipeline);
                return Task.FromResult<DeployResult>(res);
            },
            _ =>
            {
                var res = new DeployFailure();
                return Task.FromResult<DeployResult>(res);
            }
        );
    }

    private static bool ResourceReservedByCurrentUser(Resource resource, HolderId currentHolder)
    {
        return resource.State == ResourceState.Reserved && resource.Holder == currentHolder;
    }

    private static bool RefIsAllowed(Ref @ref)
    {
        return @ref.Value.Contains("feature/");
    }
}