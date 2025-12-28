using ChatOps.App.Core.Models;

namespace ChatOps.App.UseCases.ListResources;

public interface IListResourcesUseCase
{
    Task<IReadOnlyList<Resource>> Execute(CancellationToken ct = default);
}