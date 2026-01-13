using ChatOps.Api.Features.Start;
using ChatOps.Api.Integrations.Telegram.Core;
using Moq.AutoMock;

namespace ChatOps.Api.Tests;

public class StartCommandHandlerTests
{
    private readonly StartCommandHandler _handler;
    
    public StartCommandHandlerTests()
    {
        var mocker = new AutoMocker();
        _handler = mocker.CreateInstance<StartCommandHandler>();
    }
    
    [Fact]
    public async Task ShouldReturnHelpMessage()
    {
        const string expectedMessage = """
                                       Привет. 
                                       Меня зовут Антонио, я - ChatOps.
                                       Давай накатывать вместе!

                                       Чтобы узнать, что я умею, напиши <code>/help</code>
                                       """;        
        var result = await _handler.Handle(TelegramCommand.Empty(new TelegramUser(888, "user")));
        
        Assert.True(result.TryPickT0(out var reply, out _));
        Assert.Equal(expectedMessage, reply.Text);
    }
}