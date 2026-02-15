using ChatOps.Api.Integrations;

namespace ChatOps.Api.Tests;

public class ConstantsTests
{
    [Fact]
    public void CommandPrefixShouldBeCorrect()
    {
        Assert.Equal("@chatops", Constants.CommandPrefix);
    }
}