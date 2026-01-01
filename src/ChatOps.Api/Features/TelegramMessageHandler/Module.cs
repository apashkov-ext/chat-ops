using ChatOps.Api.Features.TelegramMessageHandler.Handling;
using ChatOps.Api.Features.TelegramMessageHandler.Telegram;
using ChatOps.App.Ports;
using ChatOps.App.UseCases.ListResources;
using ChatOps.App.UseCases.ReleaseResource;
using ChatOps.App.UseCases.TakeResource;
using ChatOps.Infra.Adapters.InMemory;
using Microsoft.Extensions.Options;
using Telegram.Bot.Polling;

namespace ChatOps.Api.Features.TelegramMessageHandler;

internal static class Module
{
    public static void AddTelegramMessageHandlerFeature(this WebApplicationBuilder builder)
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
        builder.Services.AddTransient<ITelegramMessageHandler, Handling.TelegramMessageHandler>();
        builder.Services.AddTransient<ITelegramChatApi, TelegramChatApi>();
        
        builder.Services.AddHostedService<TelegramPoller>();

        builder.AddListResourcesUseCase();
        builder.AddTakeResourceUseCase();
        builder.AddReleaseResourceUseCase();
    }

    private static void AddListResourcesUseCase(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<IListResourcesUseCase, ListResourcesUseCase>();
        builder.Services.AddSingleton<IGetResources, InMemoryGetResources>();
    }    
    
    private static void AddTakeResourceUseCase(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<ITakeResourceUseCase, TakeResourceUseCase>();
    }    
    
    private static void AddReleaseResourceUseCase(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<IReleaseResourceUseCase, ReleaseResourceUseCase>();
    }
}