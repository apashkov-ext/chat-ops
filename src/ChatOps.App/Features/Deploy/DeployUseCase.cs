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
    private readonly IFindRef _findBranch;
    private readonly ICreatePipeline _createPipeline;

    public DeployUseCase(
        IFindResourceById findResourceById,
        IFindRef findBranch,
        ICreatePipeline createPipeline)
    {
        _findResourceById = findResourceById;
        _findBranch = findBranch;
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
        
        var branch = await _findBranch.Execute(@ref, ct);
        if (branch is null)
        {
            return new DeployRefNotFound();
        }
        
        var createPipeline = await _createPipeline.Execute(resource, @ref, ct);
        return await createPipeline.Match<Task<DeployResult>>(success =>
            {
                throw new NotImplementedException();
            },
            alreadyExists =>
            {
                throw new NotImplementedException();
            },
            failure =>
            {
                throw new NotImplementedException();
            }
        );

        // TODO: сколько максимум pipeline можно запускать на одном ресурсе?

        return new DeploySuccess();
    }

    private static bool ResourceReservedByCurrentUser(Resource resource, HolderId currentHolder)
    {
        return resource.State != ResourceState.Reserved && resource.Holder == currentHolder;
    }

    private static bool RefIsAllowed(Ref @ref)
    {
        return @ref.Value.Contains("feature/");
    }
}