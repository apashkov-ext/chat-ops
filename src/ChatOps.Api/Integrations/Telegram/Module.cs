using ChatOps.Api.Integrations.Telegram.Core;
using Microsoft.Extensions.Options;
using Telegram.Bot.Polling;

namespace ChatOps.Api.Integrations.Telegram;

internal static class Module
{
    public static void AddTelegramIntegration(this WebApplicationBuilder builder)
    {
        builder.Services.AddOptionsWithValidateOnStart<TelegramConfig>()
            .BindConfiguration(TelegramConfig.SectionName)
            .ValidateDataAnnotations();
        
        builder.Services.AddSingleton<ITelegramBotClient>(prov =>
        {
            var config = prov.GetRequiredService<IOptions<TelegramConfig>>().Value;
            var cli = new TelegramBotClient(config.Token);
            return cli;
        });
        
        builder.Services.AddTransient<IUpdateHandler, UpdateHandler>();
        builder.Services.AddTransient<ITelegramChatApi, TelegramChatApi>();
        builder.Services.AddHostedService<TelegramPoller>();
    }

    public static void AddInMemoryUsersCache(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<List<TelegramUser>>(_ =>
        [
            new TelegramUser(888, "Алексей", null, "apashkov")
        ]);
        builder.Services.AddTransient<IUsersCache, UsersCache>();
    }
}