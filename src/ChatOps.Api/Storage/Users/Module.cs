using ChatOps.Api.Integrations.Telegram.Core;

namespace ChatOps.Api.Storage.Users;

internal static class Module
{
    public static void AddInMemoryUsersCache(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<List<TelegramUser>>(_ => []);
        builder.Services.AddTransient<IUsersCache, UsersCache>();
    }
}