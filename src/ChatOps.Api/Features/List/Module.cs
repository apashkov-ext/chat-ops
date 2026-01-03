using ChatOps.Api.Integrations.Telegram.Core;
using ChatOps.App.Ports;
using ChatOps.App.UseCases.ListResources;
using ChatOps.Infra.Adapters.InMemory;

namespace ChatOps.Api.Features.List;

internal static class Module
{
    public static void AddListFeature(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<ITelegramCommandHandler, ListCommandHandler>();
        builder.Services.AddTransient<IListResourcesUseCase, ListResourcesUseCase>();
        builder.Services.AddSingleton<IGetResources, InMemoryGetResources>();
    }    
}