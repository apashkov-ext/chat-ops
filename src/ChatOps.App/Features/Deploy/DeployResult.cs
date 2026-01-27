global using DeployResult = OneOf.OneOf<
    ChatOps.App.Features.Deploy.DeploySuccess,
    ChatOps.App.Features.Deploy.DeployResourceNotFound,
    ChatOps.App.Features.Deploy.DeployResourceNotReserved,
    ChatOps.App.Features.Deploy.DeployBranchNotFound,
    ChatOps.App.Features.Deploy.DeployFailure
>;

namespace ChatOps.App.Features.Deploy;

public sealed record DeploySuccess;
public sealed record DeployResourceNotFound;
public sealed record DeployResourceNotReserved;
public sealed record DeployBranchNotFound;
public sealed record DeployFailure(string Error);