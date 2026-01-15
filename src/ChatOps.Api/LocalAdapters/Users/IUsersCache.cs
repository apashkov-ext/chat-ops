using ChatOps.Api.Integrations.Telegram.Core;

namespace ChatOps.Api.LocalAdapters.Users;

internal interface IUsersCache
{
    void Set(TelegramUser user);
    TelegramUser? Find(long id);
}