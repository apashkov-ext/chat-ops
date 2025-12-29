using ChatOps.App.Core.Models;

namespace ChatOps.App.UseCases.ListResources;

public interface IListResourcesUseCase
{
    Task<IReadOnlyList<ResourceInfo>> Execute(CancellationToken ct = default);
}

public sealed record ResourceInfo(Resource Resource, Holder? Holder);

internal sealed class ListResourcesUseCase : IListResourcesUseCase
{
    public async Task<IReadOnlyList<ResourceInfo>> Execute(CancellationToken ct = default)
    {
        return
        [
            new(new Resource
            {
                Id = new ResourceId(Guid.NewGuid().ToString()),
                Name = "dev",
                State = ResourceState.Free
            }, null),
            new(new Resource
            {
                Id = new ResourceId(Guid.NewGuid().ToString()),
                Name = "dev1",
                State = ResourceState.Reserved
            }, new Holder
            {
                Id = new HolderId(Guid.NewGuid().ToString()),
                Name = "@user"
            }),
            new(new Resource
            {
                Id = new ResourceId(Guid.NewGuid().ToString()),
                Name = "dev2",
                State = ResourceState.Free
            }, null)
        ];
    }
}