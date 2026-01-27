using ChatOps.Api.Integrations.Telegram;
using ChatOps.Infra.Integrations.GitLab;

namespace ChatOps.Api.Integrations;

internal static class Module
{
    public static void AddIntegrations(this WebApplicationBuilder builder)
    {
        builder.AddTelegramIntegration();
        builder.Services.AddGitLabIntegration(builder.Configuration);
    }
}