using ChatOps.Api.Adapters.BackgroundServices;
using ChatOps.Api.Integrations.Telegram;
using ChatOps.App.Extensions;
using ChatOps.Infra.Extensions;

namespace ChatOps.Api.Extensions;

internal static class ServiceCollectionExtensions
{
    public static void RegisterServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddHostedService<TelegramPoller>();
        services.RegisterTelegramServices(configuration);
        services.RegisterApplicationServices(configuration);
        services.RegisterInfrastructureServices(configuration);
    }
}