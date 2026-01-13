using ChatOps.App.Core.Models;
using ChatOps.App.SharedPorts;

namespace ChatOps.App.Features.Take;

public interface ITakeResourceUseCase
{
    Task<OneOf.OneOf<TakeResourceSuccess, TakeResourceFailure>> Execute(
        HolderId holderId,
        string resourceName, 
        CancellationToken ct = default);
}

public sealed record TakeResourceSuccess(string Reply)
{
    public static implicit operator TakeResourceSuccess(string reply) => new(reply);
}

public sealed record TakeResourceFailure(string Error);

public sealed class TakeResourceUseCase : ITakeResourceUseCase
{
    private readonly IFindResourceById _findResourceById;

    public TakeResourceUseCase(IFindResourceById findResourceById)
    {
        _findResourceById = findResourceById;
    }
    
    public async Task<OneOf.OneOf<TakeResourceSuccess, TakeResourceFailure>> Execute(HolderId holderId, string resourceName, CancellationToken ct = default)
    {
        return new TakeResourceFailure("Фича еще не готова");
    }
}