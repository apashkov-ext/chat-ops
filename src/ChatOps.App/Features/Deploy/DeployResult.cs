global using DeployResult = OneOf.OneOf<
    ChatOps.App.Features.Deploy.DeploySuccess,
    ChatOps.App.Features.Deploy.DeployResourceNotFound,
    ChatOps.App.Features.Deploy.DeployResourceNotReserved,
    ChatOps.App.Features.Deploy.DeployRefNotFound,
    ChatOps.App.Features.Deploy.DeployInProcess,
    ChatOps.App.Features.Deploy.DeployFailure
>;
using ChatOps.App.Core.Models;

namespace ChatOps.App.Features.Deploy;

public sealed record DeploySuccess(Pipeline Pipeline);
public sealed record DeployResourceNotFound;
public sealed record DeployResourceNotReserved;
public sealed record DeployRefNotFound;
public sealed record DeployInProcess(Pipeline Pipeline);
public sealed record DeployFailure(DeployFailureReason Reason);

public enum DeployFailureReason
{
    Unknown,
    /// <summary>
    /// Проблемы с интеграцией
    /// </summary>
    IncorrectIntegration,
    /// <summary>
    /// Временная ошибка
    /// </summary>
    PleaseTryAgain
}