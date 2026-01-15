using ChatOps.Api.Integrations.Telegram.Core;

namespace ChatOps.Api.Storage.Users;

internal sealed class UsersCache : IUsersCache
{
    private readonly List<TelegramUser> _users;

    public UsersCache(List<TelegramUser> users)
    {
        _users = users;
    }
    
    public void Set(TelegramUser user)
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

    public TelegramUser? Find(long id)
    {
        return _users.Find(x => x.Id == id);
    }
}