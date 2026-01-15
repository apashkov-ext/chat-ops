using ChatOps.Api.LocalAdapters;
using ChatOps.Infra.Integrations.InMemoryDatabase;
using ChatOps.Infra.SharedAdapters;

namespace ChatOps.Api.Extensions;

internal static class WebApplicationBuilderExtensions
{
    public static void AddAdapters(this WebApplicationBuilder builder)
    {
        builder.AddLocalAdapters();
        builder.Services.AddSharedAdapters();
        builder.Services.AddInMemoryDatabase();
    }    
}