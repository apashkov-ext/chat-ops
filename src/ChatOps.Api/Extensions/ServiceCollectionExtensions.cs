using ChatOps.Api.Adapters.BackgroundServices;
using ChatOps.Api.Integrations.Telegram;
using ChatOps.App.Extensions;

namespace ChatOps.Api.Extensions;

internal static class ServiceCollectionExtensions
{
    public static void RegisterServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddHostedService<TelegramPoller>();
        services.RegisterApplicationServices(configuration);
        services.RegisterTelegramServices(configuration);
    }
}