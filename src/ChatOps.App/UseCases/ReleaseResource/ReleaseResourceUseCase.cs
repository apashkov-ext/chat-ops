namespace ChatOps.App.UseCases.ReleaseResource;

public interface IReleaseResourceUseCase
{
    Task<ReleaseResourceResult> Execute(string name, CancellationToken ct = default);
}

public record ReleaseResourceResult;

internal sealed class ReleaseResourceUseCase : IReleaseResourceUseCase
{
    public Task<ReleaseResourceResult> Execute(string name, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}