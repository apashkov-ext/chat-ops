using ChatOps.Api.Integrations.Telegram.Core;

namespace ChatOps.Api.Features.Start;

internal static class Module
{
    public static void AddStartFeature(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<ITelegramCommandHandler, StartCommandHandler>();
        builder.Services.AddTransient<ICommandInfo, StartCommandHandler>();
    }
}