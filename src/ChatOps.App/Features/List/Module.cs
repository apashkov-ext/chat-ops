using Microsoft.Extensions.DependencyInjection;

namespace ChatOps.App.Features.List;

public static class Module
{
    public static IServiceCollection AddListFeatureApp(this IServiceCollection services)
    {
        services.AddTransient<IListResourcesUseCase, ListResourcesUseCase>();
        return services;
    }    
}