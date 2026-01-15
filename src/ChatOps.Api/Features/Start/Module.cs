using ChatOps.Api.Integrations.Telegram.Core;

namespace ChatOps.Api.Features.Start;

internal static class Module
{
    public static void AddStartFeature(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<StartCommandHandler>();
        builder.Services.AddTransient<ITelegramCommandHandler>(prov => prov.GetRequiredService<StartCommandHandler>());
        builder.Services.AddTransient<ICommandInfo>(prov  => prov.GetRequiredService<StartCommandHandler>());
    }
}