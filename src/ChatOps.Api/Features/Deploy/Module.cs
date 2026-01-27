using ChatOps.Api.Integrations.Telegram.Core;
using ChatOps.App.Features.Deploy;

namespace ChatOps.Api.Features.Deploy;

internal static class Module
{
    public static void AddDeployFeature(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<DeployCommandHandler>();
        builder.Services.AddTransient<ITelegramCommandHandler>(prov => prov.GetRequiredService<DeployCommandHandler>());
        builder.Services.AddTransient<ICommandInfo>(prov => prov.GetRequiredService<DeployCommandHandler>());
        builder.Services.AddDeployFeatureApp();
    }    
}