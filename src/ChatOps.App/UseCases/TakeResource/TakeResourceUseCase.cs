namespace ChatOps.App.UseCases.TakeResource;

public interface ITakeResourceUseCase
{
    Task<TakeResourceResult> Execute(string name, CancellationToken ct = default);
}

public record TakeResourceResult;