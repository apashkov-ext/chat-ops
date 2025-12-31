using ChatOps.App.Core.Models;
using ChatOps.App.Ports;

namespace ChatOps.App.UseCases.ListResources;

public interface IListResourcesUseCase
{
    Task<IReadOnlyList<Resource>> Execute(CancellationToken ct = default);
}

internal sealed class ListResourcesUseCase : IListResourcesUseCase
{
    private readonly IGetResources _getResources;

    public ListResourcesUseCase(IGetResources getResources)
    {
        _getResources = getResources;
    }
    
    public async Task<IReadOnlyList<Resource>> Execute(CancellationToken ct = default)
    {
        var resources = await _getResources.Execute(ct);
        return resources;
    }
}