using ChatOps.Api.Integrations.Telegram.Core;
using ChatOps.Api.Tests.TestData;

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