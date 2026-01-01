using ChatOps.Api.Features.TelegramMessageHandler;

namespace ChatOps.Api.Extensions;

internal static class WebApplicationBuilderExtensions
{
    public static void RegisterServices(this WebApplicationBuilder builder)
    {
        builder.AddFeatures();
    }

    private static void AddFeatures(this WebApplicationBuilder builder)
    {
        builder.AddTelegramMessageHandlerFeature();
    }
}