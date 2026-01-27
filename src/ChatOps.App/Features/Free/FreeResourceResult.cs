global using FreeResourceResult = OneOf.OneOf<
    ChatOps.App.Features.Free.FreeResourceSuccess,
    ChatOps.App.Features.Free.FreeResourceNotFound,
    ChatOps.App.Features.Free.FreeResourceInUse,
    ChatOps.App.Features.Free.FreeResourceAlreadyFree,
    ChatOps.App.Features.Free.FreeResourceFailure
>;
using ChatOps.App.Core.Models;

namespace ChatOps.App.Features.Free;

public sealed record FreeResourceSuccess;
public sealed record FreeResourceNotFound;
public sealed record FreeResourceInUse(HolderId HolderId);
public sealed record FreeResourceAlreadyFree;
public sealed record FreeResourceFailure(string Error);
