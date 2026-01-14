using ChatOps.App.Core.Models;

namespace ChatOps.App.Features.Free;

public interface IFreeResourceUseCase
{
    Task<FreeResourceResult> Execute(
        HolderId holder,
        ResourceId resourceId,
        CancellationToken ct = default);
}

public sealed class FreeResourceUseCase : IFreeResourceUseCase
{
    public async Task<FreeResourceResult> Execute(
        HolderId holder,
        ResourceId resourceId,
        CancellationToken ct = default)
    {
        return new FreeResourceFailure("Фича еще не готова");
    }
}