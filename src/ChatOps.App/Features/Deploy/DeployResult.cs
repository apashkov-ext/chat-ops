global using DeployResult = OneOf.OneOf<
    ChatOps.App.Features.Deploy.DeploySuccess,
    ChatOps.App.Features.Deploy.DeployResourceNotFound,
    ChatOps.App.Features.Deploy.DeployResourceInUse,
    ChatOps.App.Features.Deploy.DeployResourceNotReserved,
    ChatOps.App.Features.Deploy.DeployRefNotFound,
    ChatOps.App.Features.Deploy.DeployFailure
>;
using ChatOps.App.Core.Models;

namespace ChatOps.App.Features.Deploy;

public sealed record DeploySuccess;
public sealed record DeployResourceNotFound;
public sealed record DeployResourceInUse(HolderId holder);
public sealed record DeployResourceNotReserved;
public sealed record DeployRefNotFound;
public sealed record DeployFailure(string Error);