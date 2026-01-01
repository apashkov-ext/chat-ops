using ChatOps.Api.Features.Env.List;
using ChatOps.Api.Features.Env.Release;
using ChatOps.Api.Features.Env.Take;

namespace ChatOps.Api.Features.Env;

internal static class Module
{
    public static void AddEnvFeature(this WebApplicationBuilder builder)
    {
        builder.AddListFeature();
        builder.AddReleaseFeature();
        builder.AddTakeFeature();
    }
}