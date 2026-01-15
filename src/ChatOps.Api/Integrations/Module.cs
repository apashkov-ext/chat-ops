using ChatOps.Api.Integrations.Telegram;

namespace ChatOps.Api.Integrations;

internal static class Module
{
    public static void AddIntegrations(this WebApplicationBuilder builder)
    {
        builder.AddTelegramIntegration();
    }
}