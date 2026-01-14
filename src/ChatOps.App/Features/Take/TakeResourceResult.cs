global using TakeResourceResult = OneOf.OneOf<
    ChatOps.App.Features.Take.TakeResourceSuccess,
    ChatOps.App.Features.Take.TakeResourceNotFound,
    ChatOps.App.Features.Take.TakeResourceAlreadyReserved,
    ChatOps.App.Features.Take.TakeResourceFailure
>;
using ChatOps.App.Core.Models;

namespace ChatOps.App.Features.Take;

public sealed record TakeResourceSuccess;
public sealed record TakeResourceNotFound;
public sealed record TakeResourceAlreadyReserved(HolderId HolderId); 
public sealed record TakeResourceFailure(string Error);