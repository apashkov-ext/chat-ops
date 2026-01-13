namespace ChatOps.Api.Integrations.Telegram.Core;

internal interface IUsersCache
{
    void Set(TelegramUser user);
    TelegramUser? Find(long id);
}