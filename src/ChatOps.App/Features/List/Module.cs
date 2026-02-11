using Microsoft.Extensions.DependencyInjection;

namespace ChatOps.App.Features.List;

public static class Module
{
    public static void AddListFeatureApp(this IServiceCollection services)
    {
        services.AddTransient<IListResourcesUseCase, ListResourcesUseCase>();
    }    
}