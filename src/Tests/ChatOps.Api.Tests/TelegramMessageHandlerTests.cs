using ChatOps.Api.Integrations.Telegram.Handling;
using Moq.AutoMock;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ChatOps.Api.Tests;

public class TelegramMessageHandlerTests
{
    private readonly TelegramMessageHandler _handler;
    
    public TelegramMessageHandlerTests()
    {
        var mocker = new AutoMocker();
        _handler = mocker.CreateInstance<TelegramMessageHandler>();
    }
    
    [Fact]
    public async Task Handle_NonTextMessageType_ShouldReturnFailure()
    {
        var message = new Message
        {
            VideoNote = new VideoNote()
        };

        var result = await _handler.Handle(message);
        
        Assert.True(result.TryPickT1(out var failure, out _));
        Assert.Equal("Unsupported message type", failure.Error);
    }
    
    [Fact]
    public async Task Handle_FromIsNull_ShouldReturnFailure()
    {
        var message = new Message
        {
            Text = "Message"
        };

        var result = await _handler.Handle(message);
        
        Assert.True(result.TryPickT1(out var failure, out _));
        Assert.Equal("FROM is null", failure.Error);
    }    
    
    [Fact]
    public async Task Handle_EmptyText_ShouldReturnSuccess()
    {
        var message = new Message
        {
            Text = "  ",
            From = new User
            {
                Id = 888,
                FirstName = "Jon",
                LastName = "Snow"
            }
        };

        var result = await _handler.Handle(message);
        
        Assert.True(result.TryPickT0(out var reply, out _));
        Assert.Equal("Введите /help для отображения доступных команд", reply.Text);
    }
    
    [Fact]
    public async Task Handle_Start_ShouldReturnGreetingMessage()
    {
        var message = new Message
        {
            Text = "/start",
            From = new User
            {
                Id = 888,
                FirstName = "Jon",
                LastName = "Snow"
            }
        };

        var result = await _handler.Handle(message);
        
        Assert.True(result.TryPickT0(out var reply, out _));
        Assert.Equal("Будем знакомы", reply.Text);
    }    
    
    [Fact]
    public async Task Handle_Help_ShouldReturnHelpMessage()
    {
        var message = new Message
        {
            Text = "/help",
            From = new User
            {
                Id = 888,
                FirstName = "Jon",
                LastName = "Snow"
            }
        };

        var result = await _handler.Handle(message);
        
        Assert.True(result.TryPickT0(out var reply, out _));
        Assert.StartsWith("<b>Доступные команды</b>", reply.Text);
    }
}