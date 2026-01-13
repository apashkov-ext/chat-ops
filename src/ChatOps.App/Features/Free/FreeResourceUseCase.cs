namespace ChatOps.App.Features.Free;

public interface IFreeResourceUseCase
{
    Task<FreeResourceResult> Execute(string name, CancellationToken ct = default);
}

public record FreeResourceResult;

public sealed class FreeResourceUseCase : IFreeResourceUseCase
{
    public Task<FreeResourceResult> Execute(string name, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}