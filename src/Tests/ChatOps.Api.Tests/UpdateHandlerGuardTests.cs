using ChatOps.Api.Integrations.Telegram;
using ChatOps.Api.Tests.TestData;
using Microsoft.Extensions.Options;
using Moq;
using Moq.AutoMock;
using Telegram.Bot.Types;

namespace ChatOps.Api.Tests;

public class UpdateHandlerGuardTests
{
    private readonly Mock<IOptions<TelegramConfig>> _options;
    private readonly UpdateHandlerGuard _guard;
    
    public UpdateHandlerGuardTests()
    {
        var mocker = new AutoMocker();

        _options = new Mock<IOptions<TelegramConfig>>();
        var config = new TelegramConfig
        {
            Token = "token",
            AllowedChats = string.Empty
        };
        _options.Setup(x => x.Value).Returns(config);
        mocker.Use(_options);
        
        _guard = mocker.CreateInstance<UpdateHandlerGuard>();
    }
    
    [Fact]
    public void CanHandle_MessageIsNull_ShouldReturnFalse()
    {
        var update = new Update();
        
        var can = _guard.CanHandle(update);
        
        Assert.False(can);
    }    
    
    [Fact]
    public void CanHandle_ChatIsNull_ShouldReturnFalse()
    {
        var update = new Update
        {
            Message = new Message()
        };
        
        var can = _guard.CanHandle(update);
        
        Assert.False(can);
    }     
    
    [Fact]
    public void CanHandle_ChatIsNotAllowed_ShouldReturnFalse()
    {
        var update = new Update
        {
            Message = new Message
            {
                Chat = new Chat()
            }
        };
        
        var can = _guard.CanHandle(update);
        
        Assert.False(can);
    }    
    
    [Theory, WrongMessageData]
    public void CanHandle_TypeIsWrong_ShouldReturnFalse(Message message)
    {
        var update = new Update
        {
            Message = message
        };
        update.Message.Chat = new Chat
        {
            Id = 123
        };

        var config = new TelegramConfig
        {
            Token = "token",
            AllowedChats = "123"
        };
        _options.Setup(x => x.Value).Returns(config);
        
        var can = _guard.CanHandle(update);
        
        Assert.False(can);
    }
    
    [Theory, CorrectMessageData]
    public void CanHandle_TypeIsCorrect_ShouldReturnTrue(Message message)
    {
        var update = new Update
        {
            Message = message
        };
        update.Message.Chat = new Chat
        {
            Id = 123
        };

        var config = new TelegramConfig
        {
            Token = "token",
            AllowedChats = "123"
        };
        _options.Setup(x => x.Value).Returns(config);
        
        var can = _guard.CanHandle(update);
        
        Assert.False(can);
    }
}