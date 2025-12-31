using ChatOps.Api.Integrations.Telegram.Handling;
using Microsoft.Extensions.Options;
using Telegram.Bot.Polling;

namespace ChatOps.Api.Integrations.Telegram;

internal static class ServiceCollectionExtensions
{
    public static void RegisterTelegramServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddOptionsWithValidateOnStart<TelegramConfig>()
            .BindConfiguration(TelegramConfig.SectionName)
            .ValidateDataAnnotations();
        
        services.AddSingleton<ITelegramBotClient>(prov =>
        {
            var config = prov.GetRequiredService<IOptions<TelegramConfig>>().Value;
            var cli = new TelegramBotClient(config.Token);
            return cli;
        });
        
        services.AddTransient<IUpdateHandler, UpdateHandler>();
        services.AddTransient<ITelegramMessageHandler, TelegramMessageHandler>();
        services.AddTransient<ITelegramChatApi, TelegramChatApi>();
    }
}