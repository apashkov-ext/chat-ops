using ChatOps.Api.Features.Deploy;
using ChatOps.Api.Features.Free;
using ChatOps.Api.Features.Help;
using ChatOps.Api.Features.List;
using ChatOps.Api.Features.Start;
using ChatOps.Api.Features.Take;

namespace ChatOps.Api.Features;

internal static class Module
{
    public static void AddFeatures(this WebApplicationBuilder builder)
    {
        builder.AddStartFeature();
        builder.AddHelpFeature();
        builder.AddListFeature();
        builder.AddTakeFeature();
        builder.AddFreeFeature();
        builder.AddDeployFeature();
    }
}