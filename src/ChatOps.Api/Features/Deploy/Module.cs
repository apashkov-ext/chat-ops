using ChatOps.Api.Integrations.Telegram.Core;
using ChatOps.App.Features.Deploy;
using ChatOps.Infra.Features.Deploy;

namespace ChatOps.Api.Features.Deploy;

internal static class Module
{
    public static void AddDeployFeature(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<DeployCommandHandler>()
            .AddTransient<ITelegramCommandHandler>(prov => prov.GetRequiredService<DeployCommandHandler>())
            .AddTransient<ICommandInfo>(prov => prov.GetRequiredService<DeployCommandHandler>());
        
        builder.Services.AddDeployFeatureApp();
        builder.Services.AddDeployFeatureInfra();
    }    
}