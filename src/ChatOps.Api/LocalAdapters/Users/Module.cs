using ChatOps.Api.Integrations.Telegram.Core;

namespace ChatOps.Api.LocalAdapters.Users;

internal static class Module
{
    public static void AddUsersCache(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<List<TelegramUser>>(_ => []);
        builder.Services.AddTransient<UsersStorage>();
        builder.Services.AddTransient<IFindTelegramUserById>(prov => prov.GetRequiredService<UsersStorage>());
        builder.Services.AddTransient<IUpsertTelegramUser>(prov => prov.GetRequiredService<UsersStorage>());
    }
}