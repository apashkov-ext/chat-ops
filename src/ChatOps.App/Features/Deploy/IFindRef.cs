using ChatOps.App.Core.Models;

using FindRefResult = OneOf.OneOf<
    ChatOps.App.Features.Deploy.FindRefSuccess,
    ChatOps.App.Features.Deploy.FindRefNotFound,
    ChatOps.App.Features.Deploy.FindRefFailure
>;

namespace ChatOps.App.Features.Deploy;

public interface IFindRef
{
    Task<FindRefResult> Execute(RefName refName, CancellationToken ct = default);
}

public sealed record FindRefSuccess(Ref Ref);
public sealed record FindRefNotFound;
public sealed record FindRefFailure(FindRefFailureReason Reason); 

public enum FindRefFailureReason
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