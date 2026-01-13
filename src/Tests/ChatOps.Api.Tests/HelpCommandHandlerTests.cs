using ChatOps.Api.Features.Help;
using ChatOps.Api.Integrations.Telegram.Core;
using Moq;
using Moq.AutoMock;

namespace ChatOps.Api.Tests;

public class HelpCommandHandlerTests
{
    private readonly HelpCommandHandler _handler;
    private readonly List<ICommandInfo> _infos;
    
    public HelpCommandHandlerTests()
    {
        var mocker = new AutoMocker();

        _infos = new List<ICommandInfo>();
        mocker.Use<IEnumerable<ICommandInfo>>(_infos);
        
        _handler = mocker.CreateInstance<HelpCommandHandler>();
    }
    
    [Fact]
    public async Task ShouldReturnSelfHelpMessage()
    {
        const string expectedMessage = """
                                       <b>Доступные команды:</b>

                                        <b>help</b>
                                        Показать справку
                                       """;
        
        var result = await _handler.Handle(TelegramCommand.Empty(new TelegramUser(888, "user")));
        
        Assert.True(result.TryPickT0(out var reply, out _));
        Assert.Equal(expectedMessage, reply.Text);
    }
    
    [Fact]
    public async Task ShouldReturnHelpMessage()
    {
        const string expectedMessage = """
                                       <b>Доступные команды:</b>
                                       
                                        <b>start</b>
                                        Поздороваться

                                        <b>help</b>
                                        Показать справку
                                       """;
        
        var infoMock = new Mock<ICommandInfo>();
        infoMock.SetupGet(x => x.Command).Returns("start");
        infoMock.SetupGet(x => x.Description).Returns("Поздороваться");
        _infos.Add(infoMock.Object);
        
        var result = await _handler.Handle(TelegramCommand.Empty(new TelegramUser(888, "user")));
        
        Assert.True(result.TryPickT0(out var reply, out _));
        Assert.Equal(expectedMessage, reply.Text);
    }
}