using ChatOps.Api.Integrations.Telegram.Core;
using ChatOps.Api.Tests.TestData;

namespace ChatOps.Api.Tests;

public class TelegramCommandTests
{
    private readonly TelegramUser _user = new (123, "John", "Snow");
    
    [Theory, EmptyStringData]
    public void Parse_EmptyInput_ShouldReturnEmptyCommand(string input)
    {
        var cmd = TelegramCommand.Parse(_user, input);
        Assert.Equal(TelegramCommand.Empty(_user), cmd);
    }
    
    [Fact]
    public void Parse_ShouldReturnCommand()
    {
        const string input = " my  command  ";
        
        var cmd = TelegramCommand.Parse(_user, input);
        
        Assert.Equal(2, cmd.Tokens.Count);
        Assert.Equal("my", cmd.Tokens[0]);
        Assert.Equal("command", cmd.Tokens[1]);
    }
}