using ChatOps.Api.Integrations.Telegram.Core;
using ChatOps.App.Features.List;

namespace ChatOps.Api.Features.List;

internal static class Module
{
    public static void AddListFeature(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddTransient<ITelegramCommandHandler, ListCommandHandler>()
            .AddTransient<ICommandInfo, ListCommandHandler>()
            .AddListFeatureApp();
    }    
}