using ChatOps.Api.Integrations.Telegram.Core;

namespace ChatOps.Api.Features.Help;

internal static class Module
{
    public static void AddHelpFeature(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<ITelegramCommandHandler, HelpCommandHandler>();
    }
}