using ChatOps.Api.LocalAdapters.Files;
using ChatOps.Api.LocalAdapters.Users;

namespace ChatOps.Api.LocalAdapters;

internal static class Module
{
    public static void AddLocalAdapters(this WebApplicationBuilder builder)
    {
        builder.AddUsersCache();
        builder.AddImagesDatabase();
    }
}