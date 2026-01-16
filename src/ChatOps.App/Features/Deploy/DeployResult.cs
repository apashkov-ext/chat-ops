global using DeployResult = OneOf.OneOf<
    ChatOps.App.Features.Deploy.DeploySuccess,
    ChatOps.App.Features.Deploy.DeployResourceNotFound,
    ChatOps.App.Features.Deploy.DeployResourceInUse,
    ChatOps.App.Features.Deploy.DeployBranchNotFound,
    ChatOps.App.Features.Deploy.DeployFailure
>;
using ChatOps.App.Core.Models;

namespace ChatOps.App.Features.Deploy;

public sealed record DeploySuccess;
public sealed record DeployResourceNotFound;
public sealed record DeployResourceInUse(HolderId HolderId);
public sealed record DeployBranchNotFound;
public sealed record DeployFailure(string Error);