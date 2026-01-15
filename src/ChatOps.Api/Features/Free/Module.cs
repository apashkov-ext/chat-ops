using ChatOps.Api.Integrations.Telegram.Core;
using ChatOps.App.Features.Free;

namespace ChatOps.Api.Features.Free;

internal static class Module
{
    public static void AddFreeFeature(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<FreeCommandHandler>();
        builder.Services.AddTransient<ITelegramCommandHandler>(prov => prov.GetRequiredService<FreeCommandHandler>());
        builder.Services.AddTransient<ICommandInfo>(prov => prov.GetRequiredService<FreeCommandHandler>());
        builder.Services.AddFreeFeatureApp();
    }    
}