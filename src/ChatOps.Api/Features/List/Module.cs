using ChatOps.Api.Integrations.Telegram.Core;
using ChatOps.App.Features.List;

namespace ChatOps.Api.Features.List;

internal static class Module
{
    public static void AddListFeature(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddTransient<ListCommandHandler>()
            .AddTransient<ITelegramCommandHandler>(prov => prov.GetRequiredService<ListCommandHandler>())
            .AddTransient<ICommandInfo>(prov => prov.GetRequiredService<ListCommandHandler>())
            .AddListFeatureApp();
    }    
}