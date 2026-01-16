using Microsoft.Extensions.DependencyInjection;

namespace ChatOps.App.Features.Deploy;

public static class Module
{
    public static void AddDeployFeatureApp(this IServiceCollection services)
    {
        services.AddTransient<IDeployUseCase, DeployUseCase>();
    }    
}