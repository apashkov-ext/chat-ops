using ChatOps.App.UseCases.ListResources;
using ChatOps.App.UseCases.ReleaseResource;
using ChatOps.App.UseCases.TakeResource;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChatOps.App.Extensions;

public static class ServiceCollectionExtensions
{
    public static void RegisterApplicationServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddTransient<IListResourcesUseCase, ListResourcesUseCase>();
        services.AddTransient<ITakeResourceUseCase, TakeResourceUseCase>();
        services.AddTransient<IReleaseResourceUseCase, ReleaseResourceUseCase>();
    }
}