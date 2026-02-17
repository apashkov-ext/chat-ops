using ChatOps.Api.Features.Start;
using ChatOps.Api.Integrations;
using ChatOps.Api.Integrations.Telegram.Core;
using Moq;
using Moq.AutoMock;

namespace ChatOps.Api.Tests;

public class StartCommandHandlerTests
{
    private readonly StartCommandHandler _handler;
    
    public StartCommandHandlerTests()
    {
        var mocker = new AutoMocker();

        var ver = new Mock<IApplicationVersionResolver>();
        ver.Setup(x => x.GetVersion()).Returns("1.0.0");
        mocker.Use(ver);
        
        _handler = mocker.CreateInstance<StartCommandHandler>();
    }
    
    [Fact]
    public async Task ShouldReturnHelpMessage()
    {
        const string expectedMessage = $"""
                                        👋 Привет. 
                                        Меня зовут Антонио, я - ChatOps.
                                        Давай накатывать вместе!

                                        Чтобы узнать, что я умею, напиши <code>{Constants.CommandPrefix} help</code>
                                        v.<code>1.0.0</code>
                                        """;        
        var result = await _handler.Handle(TelegramCommand.Empty(new TelegramUser(888, "user")));
        
        Assert.True(result.TryPickT0(out var reply, out _));
        Assert.Equal(expectedMessage, reply.Text?.Text);
    }
}