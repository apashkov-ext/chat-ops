using ChatOps.App.SharedPorts;
using Microsoft.Extensions.DependencyInjection;

namespace ChatOps.Infra.SharedAdapters;

public static class Module
{
    public static void AddSharedAdapters(this IServiceCollection services)
    {
        services.AddTransient<IGetResources, InMemoryGetResources>();
        services.AddTransient<IFindResourceById, InMemoryFindResourceById>();
        services.AddTransient<IUpdateResource, InMemoryUpdateResource>();
    }    
}