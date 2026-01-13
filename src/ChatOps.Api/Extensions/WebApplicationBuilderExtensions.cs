using ChatOps.Api.Features.Free;
using ChatOps.Api.Features.Help;
using ChatOps.Api.Features.List;
using ChatOps.Api.Features.Start;
using ChatOps.Api.Features.Take;
using ChatOps.Api.Integrations.Telegram;
using ChatOps.Infra.Integrations.InMemoryDatabase;
using ChatOps.Infra.SharedAdapters;

namespace ChatOps.Api.Extensions;

internal static class WebApplicationBuilderExtensions
{
    public static void RegisterServices(this WebApplicationBuilder builder)
    {
        builder.AddTelegramIntegration();
        builder.AddInMemoryUsersCache();
        builder.Services.AddInMemoryDatabase();
        builder.Services.AddSharedPorts();
        
        builder.AddStartFeature();
        builder.AddHelpFeature();
        builder.AddListFeature();
        builder.AddTakeFeature();
        builder.AddReleaseFeature();
    }
}