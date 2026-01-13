using Microsoft.Extensions.DependencyInjection;

namespace ChatOps.App.Features.Take;

public static class Module
{
    public static void AddTakeFeatureApp(this IServiceCollection services)
    {
        services.AddTransient<ITakeResourceUseCase, TakeResourceUseCase>();
        services.AddTransient<ITakeResourceAndDeployUseCase, TakeResourceAndDeployUseCase>();
    }    
}