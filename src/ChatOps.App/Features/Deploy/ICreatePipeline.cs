using ChatOps.App.Core.Models;
using OneOf;

namespace ChatOps.App.Features.Deploy;

public interface ICreatePipeline
{
    Task<OneOf<CreatePipelineSuccess, CreatePipelineFailure>> Execute(
        Resource resource, 
        Ref @ref, 
        IEnumerable<Variable> variables,
        CancellationToken ct = default);
}

public sealed record CreatePipelineSuccess(Pipeline Pipeline);
public sealed record CreatePipelineFailure(CreatePipelineFailureReason Reason);

public enum CreatePipelineFailureReason
{
    Unknown,
    /// <summary>
    /// Проблемы с интеграцией
    /// </summary>
    Permanent,
    /// <summary>
    /// Временная ошибка
    /// </summary>
    Transient
}
