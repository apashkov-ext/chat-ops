namespace ChatOps.Api.Integrations.Telegram.Core;

internal sealed record TelegramUser(
    long Id, 
    string FirstName, 
    string? LastName = null,
    string? Username = null)
{
    public string GetMention()
    {
        var displayName = !string.IsNullOrWhiteSpace(Username) 
            ? $"@{Username}" 
            : GetName();
        
        var mention = $"<a href=\"tg://user?id={Id}\">{displayName}</a>";
        return mention;
    }

    public static string GetMention(long userId, string displayName)
    {
        var mention = $"<a href=\"tg://user?id={userId}\">{displayName}</a>";
        return mention;
    }
    
    public override string ToString()
    {
        return $"@{Id}: {GetName()}";
    }

    private string GetName()
    {
        return string.Join(" ", new[] { FirstName, LastName }.Where(x => !string.IsNullOrWhiteSpace(x)));
    }
}