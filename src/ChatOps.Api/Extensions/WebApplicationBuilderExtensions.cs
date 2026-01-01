using ChatOps.Api.Features.Env;
using ChatOps.Api.Features.TelegramMessageHandler;

namespace ChatOps.Api.Extensions;

internal static class WebApplicationBuilderExtensions
{
    public static void RegisterServices(this WebApplicationBuilder builder)
    {
        builder.AddEnvFeature();
    }
}