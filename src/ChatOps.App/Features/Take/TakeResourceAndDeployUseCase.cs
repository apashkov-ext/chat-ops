using ChatOps.App.Core.Models;

namespace ChatOps.App.Features.Take;

public sealed record TakeResourceAndDeploySuccess(string Reply)
{
    public static implicit operator TakeResourceAndDeploySuccess(string reply) => new(reply);
}

public sealed record TakeResourceAndDeployFailure(string Error);

public interface ITakeResourceAndDeployUseCase
{
    Task<OneOf.OneOf<TakeResourceAndDeploySuccess, TakeResourceAndDeployFailure>> Execute(
        HolderId holder, 
        string resourceName,
        string branchName,
        CancellationToken ct = default
    );
}

internal sealed class TakeResourceAndDeployUseCase : ITakeResourceAndDeployUseCase
{
    public async Task<OneOf.OneOf<TakeResourceAndDeploySuccess, TakeResourceAndDeployFailure>> Execute(
        HolderId holder, 
        string resourceName, 
        string branchName,
        CancellationToken ct = default
        )
    {
        return new TakeResourceAndDeployFailure("Фича еще не готова");
    }
}