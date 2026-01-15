using ChatOps.Api.Integrations.Telegram.Core;

namespace ChatOps.Api.LocalAdapters.Users;

internal interface IUpsertTelegramUser
{
    void Upsert(TelegramUser user);
}