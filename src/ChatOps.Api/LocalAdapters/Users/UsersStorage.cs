using ChatOps.Api.Integrations.Telegram.Core;

namespace ChatOps.Api.LocalAdapters.Users;

internal sealed class UsersStorage : IFindTelegramUserById, IUpsertTelegramUser
{
    private readonly List<TelegramUser> _users;

    public UsersStorage(List<TelegramUser> users)
    {
        _users = users;
    }

    public TelegramUser? Find(long id)
    {
        return _users.Find(x => x.Id == id);
    }
    
    public void Upsert(TelegramUser user)
    {
        var index = _users.FindIndex(x => x.Id == user.Id);
        if (index != -1)
        {
            _users[index] = user;
        }
        else
        {
            _users.Add(user);
        }
    }
}