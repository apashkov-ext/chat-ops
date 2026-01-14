using ChatOps.Api.Features.Free;
using ChatOps.Api.Features.Help;
using ChatOps.Api.Integrations.Telegram.Core;
using Moq;
using Moq.AutoMock;

namespace ChatOps.Api.Tests;

public class HelpCommandHandlerTests
{
    private readonly AutoMocker _mocker;
    private readonly List<ICommandInfo> _infos;
    
    public HelpCommandHandlerTests()
    {
        _mocker = new AutoMocker();
        _infos = new List<ICommandInfo>();
        _mocker.Use<IEnumerable<ICommandInfo>>(_infos);
    }
    
    [Fact]
    public async Task ShouldReturnSelfHelpMessage()
    {
        const string expectedMessage = """
                                       <b>Доступные команды:</b>

                                        <b>help</b>
                                        Показать справку
                                       """;
        
        var handler = _mocker.CreateInstance<HelpCommandHandler>();
        var result = await handler.Handle(TelegramCommand.Empty(new TelegramUser(888, "user")));
        
        Assert.True(result.TryPickT0(out var reply, out _));
        Assert.Equal(expectedMessage, reply.Text);
    }
    
    [Fact]
    public async Task ShouldConsumeCommandsAndReturnHelpMessage()
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
        
        var handler = _mocker.CreateInstance<HelpCommandHandler>();
        var result = await handler.Handle(TelegramCommand.Empty(new TelegramUser(888, "user")));
        
        Assert.True(result.TryPickT0(out var reply, out _));
        Assert.Equal(expectedMessage, reply.Text);
    }
}