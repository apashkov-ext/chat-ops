global using FreeResourceResult = OneOf.OneOf<
    ChatOps.App.Features.Free.FreeResourceSuccess,
    ChatOps.App.Features.Free.FreeResourceFailure
>;

namespace ChatOps.App.Features.Free;

public sealed record FreeResourceSuccess;
public sealed record FreeResourceFailure(string Error);
