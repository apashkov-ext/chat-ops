using ChatOps.Api.Features.Deploy;
using ChatOps.Api.Integrations.Telegram.Core;

namespace ChatOps.Api.Tests;

public class TelegramCommandExtensionsTests
{
    [Fact]
    public void GetVariables_ShouldReturnEmpty()
    {
        var user = new TelegramUser(123, "First Name");
        var cmd = new TelegramCommand(user, ["cmd"]);
        
        var variables = cmd.GetVariables();
        
        Assert.Empty(variables);
    }    
    
    [Fact]
    public void GetVariables_ShouldReturnVariables()
    {
        var user = new TelegramUser(123, "First Name");
        var cmd = new TelegramCommand(user, ["cmd", "var=value", "VAR2=888"]);
        
        var variables = cmd.GetVariables().ToArray();
        
        Assert.Contains(variables, v => v is { Name: "var", Value: "value" });
        Assert.Contains(variables, v => v is { Name: "VAR2", Value: "888" });
    }
}