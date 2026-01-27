using ChatOps.App.Features.Deploy;
using Microsoft.Extensions.DependencyInjection;

namespace ChatOps.Infra.Features.Deploy;

public static class Module
{
    public static void AddDeployFeatureInfra(this IServiceCollection services)
    {
        services.AddTransient<ICreatePipeline, GitLabCreatePipeline>();
    }    
}