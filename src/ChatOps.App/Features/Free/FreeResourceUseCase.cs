using ChatOps.App.Core.Models;
using OneOf;

namespace ChatOps.App.Features.Free;

public interface IFreeResourceUseCase
{
    Task<OneOf<FreeResourceSuccess, FreeResourceFailure>> Execute(
        HolderId holder,
        string resourceName, 
        CancellationToken ct = default);
}

public sealed record FreeResourceSuccess(string Reply)
{
    public static implicit operator FreeResourceSuccess(string reply) => new(reply);
}

public sealed record FreeResourceFailure(string Error);

public sealed class FreeResourceUseCase : IFreeResourceUseCase
{
    public async Task<OneOf<FreeResourceSuccess, FreeResourceFailure>> Execute(
        HolderId holder,
        string resourceName, 
        CancellationToken ct = default)
    {
        return new FreeResourceFailure("Фича еще не готова");
    }
}