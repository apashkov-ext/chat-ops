using ChatOps.App.Core.Models;
using Microsoft.Extensions.DependencyInjection;

namespace ChatOps.Infra.Integrations.InMemoryDatabase;

public static class Module
{
    public static void AddInMemoryDatabase(this IServiceCollection services)
    {
        services.AddSingleton<List<Resource>>(_ =>
        [
            new(new ResourceId("dev"), ResourceState.Free, null),
            new(new ResourceId("dev1"), ResourceState.Reserved, new HolderId("888")),
            new(new ResourceId("dev1"), ResourceState.Free, null)
        ]);
    }    
}