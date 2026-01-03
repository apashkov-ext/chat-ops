using ChatOps.Api.Features.Help;
using ChatOps.Api.Features.List;
using ChatOps.Api.Features.Release;
using ChatOps.Api.Features.Start;
using ChatOps.Api.Features.Take;
using ChatOps.Api.Integrations.Telegram;

namespace ChatOps.Api.Extensions;

internal static class WebApplicationBuilderExtensions
{
    public static void RegisterServices(this WebApplicationBuilder builder)
    {
        builder.AddTelegramIntegration();
        builder.AddHelpFeature();
        builder.AddStartFeature();
        builder.AddListFeature();
        builder.AddTakeFeature();
        builder.AddReleaseFeature();
    }
}