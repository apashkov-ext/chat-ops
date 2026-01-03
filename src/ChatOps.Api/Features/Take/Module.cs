using ChatOps.Api.Integrations.Telegram.Core;
using ChatOps.App.UseCases.TakeResource;

namespace ChatOps.Api.Features.Take;

internal static class Module
{
    public static void AddTakeFeature(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<ITelegramCommandHandler, TakeCommandHandler>();
        builder.Services.AddTransient<ITakeResourceUseCase, TakeResourceUseCase>();
    }    
}