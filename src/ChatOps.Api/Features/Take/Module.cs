using ChatOps.Api.Integrations.Telegram.Core;
using ChatOps.App.Features.Take;
using ChatOps.Infra.Features.Take;

namespace ChatOps.Api.Features.Take;

internal static class Module
{
    public static void AddTakeFeature(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<TakeCommandHandler>();
        builder.Services.AddTransient<ITelegramCommandHandler>(prov => prov.GetRequiredService<TakeCommandHandler>());
        builder.Services.AddTransient<ICommandInfo>(prov => prov.GetRequiredService<TakeCommandHandler>());
        builder.Services.AddTakeFeatureApp();
        builder.Services.AddTakeFeatureInfra();
    }    
}