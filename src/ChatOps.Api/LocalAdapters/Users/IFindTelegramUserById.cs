using ChatOps.Api.Integrations.Telegram.Core;

namespace ChatOps.Api.LocalAdapters.Users;

internal interface IFindTelegramUserById
{
    TelegramUser? Find(long id);
}