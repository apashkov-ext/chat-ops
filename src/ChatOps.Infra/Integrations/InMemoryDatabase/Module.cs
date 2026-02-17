using ChatOps.App.Core.Models;
using Microsoft.Extensions.DependencyInjection;

namespace ChatOps.Infra.Integrations.InMemoryDatabase;

public static class Module
{
    public static void AddInMemoryDatabase(this IServiceCollection services)
    {
        services.AddSingleton<List<Resource>>(_ =>
        [
            new Resource(new ResourceId("dev"), ResourceState.Free, null),
            new Resource(new ResourceId("dev1"), ResourceState.Free, null),
            new Resource(new ResourceId("dev2"), ResourceState.Free, null)
        ]);
    }    
}