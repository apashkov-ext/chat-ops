global using CreatePipelineResult = OneOf.OneOf<
    ChatOps.App.Features.Deploy.CreatePipelineSuccess,
    ChatOps.App.Features.Deploy.CreatePipelineAlreadyExists,
    ChatOps.App.Features.Deploy.CreatePipelineFailure
>;

using ChatOps.App.Core.Models;

namespace ChatOps.App.Features.Deploy;

public interface ICreatePipeline
{
    Task<CreatePipelineResult> Execute(Resource resource, BranchId branch, CancellationToken ct = default);
}

public sealed record CreatePipelineSuccess;
public sealed record CreatePipelineAlreadyExists;
public sealed record CreatePipelineFailure;
