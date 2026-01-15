using Microsoft.Extensions.DependencyInjection;

namespace ChatOps.App.Features.Free;

public static class Module
{
    public static void AddFreeFeatureApp(this IServiceCollection services)
    {
        services.AddTransient<IFreeResourceUseCase, FreeResourceUseCase>();
    }    
}