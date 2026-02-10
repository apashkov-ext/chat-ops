global using CreatePipelineResult = OneOf.OneOf<
    ChatOps.App.Features.Deploy.CreatePipelineSuccess,
    ChatOps.App.Features.Deploy.CreatePipelineAlreadyExists,
    ChatOps.App.Features.Deploy.CreatePipelineFailure
>;

using ChatOps.App.Core.Models;

namespace ChatOps.App.Features.Deploy;

public interface ICreatePipeline
{
    Task<CreatePipelineResult> Execute(
        Resource resource, 
        Ref @ref, 
        IEnumerable<Variable> variables,
        CancellationToken ct = default);
}

public sealed record CreatePipelineSuccess(Pipeline Pipeline);
public sealed record CreatePipelineAlreadyExists(Pipeline Pipeline);
public sealed record CreatePipelineFailure;
