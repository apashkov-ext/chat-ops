namespace ChatOps.App.UseCases.ReleaseResource;

public interface IReleaseResourceUseCase
{
    Task<ReleaseResourceResult> Execute(string name, CancellationToken ct = default);
}

public record ReleaseResourceResult;

public sealed class ReleaseResourceUseCase : IReleaseResourceUseCase
{
    public Task<ReleaseResourceResult> Execute(string name, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}