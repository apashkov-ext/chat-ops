using System.Reflection;
using ChatOps.Api.Integrations.Telegram.Core;
using Xunit.Sdk;

namespace ChatOps.Api.Tests;

public class TelegramUserTests
{
    [Theory, TelegramMentionData]
    public void GetMention(string firstName, string? lastName, string? username, string expected)
    {
        const long id = 888;
        var expectedMention = $"<a href=\"tg://user?id={id}\">{expected}</a>";

        var mention = new TelegramUser(id, firstName, lastName, username).GetMention();
        
        Assert.Equal(expectedMention, mention);
    }   
    
    [Fact]
    public void GetMentionById()
    {
        const long id = 888;
        var expectedMention = $"<a href=\"tg://user?id={id}\">Человек</a>";

        var mention = TelegramUser.GetMention(888, "Человек");
        
        Assert.Equal(expectedMention, mention);
    }
}

internal sealed class TelegramMentionDataAttribute : DataAttribute
{
    public override IEnumerable<object?[]> GetData(MethodInfo testMethod)
    {
        yield return [ "Имя", string.Empty, string.Empty, "Имя" ];
        yield return [ "Имя", null, null, "Имя" ];
        
        yield return [ "Имя", "Фамилия", string.Empty, "Имя Фамилия" ];
        yield return [ "Имя", "Фамилия", null, "Имя Фамилия" ];
        
        yield return [ "Имя", string.Empty, "username", "@username" ];
        yield return [ "Имя", null, "username", "@username" ];
        yield return [ "Имя", "Фамилия", "username", "@username" ];
    }
}