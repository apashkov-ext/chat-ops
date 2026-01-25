using ChatOps.App.Features.Take;
using Microsoft.Extensions.DependencyInjection;

namespace ChatOps.Infra.Features.Take;

public static class Module
{
    public static void AddTakeFeatureInfra(this IServiceCollection services)
    {
        services.AddTransient<ICountReservedResourcesByHolder, InMemoryCountReservedResourcesByHolder>();
    }    
}