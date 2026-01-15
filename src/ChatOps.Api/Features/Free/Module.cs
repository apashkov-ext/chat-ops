using ChatOps.Api.Integrations.Telegram.Core;
using ChatOps.App.Features.Free;

namespace ChatOps.Api.Features.Free;

internal static class Module
{
    public static void AddReleaseFeature(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<ITelegramCommandHandler, FreeCommandHandler>();
        builder.Services.AddTransient<ICommandInfo, FreeCommandHandler>();
        builder.Services.AddFreeFeatureApp();
    }    
}