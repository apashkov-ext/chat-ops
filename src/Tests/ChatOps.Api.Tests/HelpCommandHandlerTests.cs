using ChatOps.Api.Features.Help;
using ChatOps.Api.Integrations.Telegram.Core;
using Moq.AutoMock;

namespace ChatOps.Api.Tests;

public class HelpCommandHandlerTests
{
    private readonly HelpCommandHandler _handler;
    
    public HelpCommandHandlerTests()
    {
        var mocker = new AutoMocker();
        _handler = mocker.CreateInstance<HelpCommandHandler>();
    }
    
    [Fact]
    public async Task ShouldReturnHelpMessage()
    {
        const string expectedMessage = """
                                       <b>Доступные команды</b>

                                        <code>list</code>
                                        <code>take &lt;env&gt; [branch]</code>
                                        <code>release &lt;env&gt;</code>
                                       """;
        
        var result = await _handler.Handle(CommandTokenCollection.Empty);
        
        Assert.True(result.TryPickT0(out var reply, out _));
        Assert.Equal(expectedMessage, reply.Text);
    }
}