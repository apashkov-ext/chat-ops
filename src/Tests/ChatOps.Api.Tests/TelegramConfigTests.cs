using ChatOps.Api.Integrations.Telegram;

namespace ChatOps.Api.Tests;

public class TelegramConfigTests
{
    [Fact]
    public void GetAllowedChatIds_SupportsCommaAndSemicolon()
    {
        var config = new TelegramConfig
        {
            Token = "token",
            AllowedChats = "-1 ;0,1 "
        };

        var ids = config.GetAllowedChatIds();
        
        Assert.Equal(-1, ids[0]);
        Assert.Equal(0, ids[1]);
        Assert.Equal(1, ids[2]);
    }    
    
    [Fact]
    public void GetAllowedChatIds_ReturnsUniqueIds()
    {
        var config = new TelegramConfig
        {
            Token = "token",
            AllowedChats = "-1;-1"
        };
        
        var ids = config.GetAllowedChatIds();
        
        Assert.Single(ids);
    }
}