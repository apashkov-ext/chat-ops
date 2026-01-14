using ChatOps.Api.Features.Take;
using ChatOps.Api.Integrations.Telegram.Core;
using ChatOps.App.Core.Models;
using ChatOps.App.Features.Take;
using Moq;
using Moq.AutoMock;

namespace ChatOps.Api.Tests;

public class TakeCommandHandlerTests
{
    private readonly TelegramUser _user;
    private readonly TakeCommandHandler _handler;
    private readonly Mock<ITakeResourceUseCase> _takeResourceUseCase;
    private readonly Mock<IUsersCache> _cache;
    
    public TakeCommandHandlerTests()
    {
        _user = new TelegramUser(888, "user");
        
        var mocker = new AutoMocker();

        _takeResourceUseCase = new Mock<ITakeResourceUseCase>();
        _takeResourceUseCase.Setup(x => x.Execute(
                It.IsAny<HolderId>(), 
                It.IsAny<ResourceId>(), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new TakeResourceSuccess());
        mocker.Use(_takeResourceUseCase);

        _cache = new Mock<IUsersCache>();
        mocker.Use(_cache);
        
        _handler = mocker.CreateInstance<TakeCommandHandler>();
    }
    
    [Fact]
    public void CanHandle_EmptyCommand_ShouldReturnFalse()
    {
        var cmd = TelegramCommand.Empty(_user);
        var can = _handler.CanHandle(cmd);
        
        Assert.False(can);
    }
    
    [Fact]
    public void CanHandle_IllegalToken_ShouldReturnFalse()
    {
        var cmd = TelegramCommand.Parse(_user, "ThisIsNotATakeToken");
        var can = _handler.CanHandle(cmd);
        
        Assert.False(can);
    }
    
    [Theory]
    [InlineData("take")]
    [InlineData("take arg1")]
    [InlineData("take arg1 argN")]
    public void CanHandle_ShouldReturnTrue(string input)
    {
        var cmd = TelegramCommand.Parse(_user, input);
        var can = _handler.CanHandle(cmd);
        
        Assert.True(can);
    }

    [Fact]
    public async Task Handle_WithoutRequiredArg_ShouldNotInvokeUseCase()
    {
        var cmd = TelegramCommand.Parse(_user, "take");
        _ = await _handler.Handle(cmd);
        
        _takeResourceUseCase.Verify(x => x.Execute(
            It.IsAny<HolderId>(),
            It.IsAny<ResourceId>(),
            It.IsAny<CancellationToken>()), Times.Never);
    }
    
    [Fact]
    public async Task Handle_WithoutRequiredArg_ShouldReturnFailure()
    {
        var cmd = TelegramCommand.Parse(_user, "take");
        var result = await _handler.Handle(cmd);
        
        Assert.True(result.TryPickT1(out var failure, out _));
        Assert.Equal("Invalid command syntax", failure.Error);
    }
    
    [Fact]
    public async Task Handle_ShouldInvokeTakeUseCase()
    {
        var cmd = TelegramCommand.Parse(_user, "take resName");
        _ = await _handler.Handle(cmd);
        
        _takeResourceUseCase.Verify(x => x.Execute(
            It.Is<HolderId>(h => h.Value == _user.Id.ToString()),
            It.Is<ResourceId>(r => r == new ResourceId("resName")),
            It.IsAny<CancellationToken>()), Times.Once);
    }    
    
    [Fact]
    public async Task Handle_SuccessShouldBeMappedToReply()
    {
        _takeResourceUseCase.Setup(x => x.Execute(
                It.IsAny<HolderId>(), 
                It.IsAny<ResourceId>(), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new TakeResourceSuccess());
        
        var cmd = TelegramCommand.Parse(_user, "take resName");
        var result = await _handler.Handle(cmd);
        
        Assert.True(result.TryPickT0(out var reply, out _));
        Assert.Equal("✅ Ресурс 'resName' зарезервирован для <a href=\"tg://user?id=888\">Личности без имени</a>", reply.Text?.Text);
    }    
    
    [Fact]
    public async Task Handle_NotFoundShouldBeMappedToReply()
    {
        _takeResourceUseCase.Setup(x => x.Execute(
                It.IsAny<HolderId>(), 
                It.IsAny<ResourceId>(), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new TakeResourceNotFound());
        
        var cmd = TelegramCommand.Parse(_user, "take resName");
        var result = await _handler.Handle(cmd);
        
        Assert.True(result.TryPickT0(out var reply, out _));
        Assert.Equal("⚠️ Ресурс не найден", reply.Text?.Text);
    }  
    
    [Fact]
    public async Task Handle_ReservedAndUserNotCached_ShouldBeMappedToReply()
    {
        _takeResourceUseCase.Setup(x => x.Execute(
                It.IsAny<HolderId>(), 
                It.IsAny<ResourceId>(), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new TakeResourceInUse(new HolderId("999")));
        
        var cmd = TelegramCommand.Parse(_user, "take resName");
        var result = await _handler.Handle(cmd);
        
        Assert.True(result.TryPickT0(out var reply, out _));
        Assert.Equal("⚠️ Ресурс уже зарезервирован для <a href=\"tg://user?id=999\">Личности без имени</a>", reply.Text?.Text);
    }      
    
    [Fact]
    public async Task Handle_ReservedAndUserCached_ShouldBeMappedToReply()
    {
        _takeResourceUseCase.Setup(x => x.Execute(
                It.IsAny<HolderId>(), 
                It.IsAny<ResourceId>(), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new TakeResourceInUse(new HolderId("999")));

        _cache.Setup(x => x.Find(It.IsAny<long>())).Returns(new TelegramUser(999, "Имя", null, "username"));
        
        var cmd = TelegramCommand.Parse(_user, "take resName");
        var result = await _handler.Handle(cmd);
        
        Assert.True(result.TryPickT0(out var reply, out _));
        Assert.Equal("⚠️ Ресурс уже зарезервирован для <a href=\"tg://user?id=999\">@username</a>", reply.Text?.Text);
    }  
    
    [Fact]
    public async Task Handle_FailureShouldBeMappedToFailure()
    {
        _takeResourceUseCase.Setup(x => x.Execute(
                It.IsAny<HolderId>(), 
                It.IsAny<ResourceId>(), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new TakeResourceFailure("error"));
        
        var cmd = TelegramCommand.Parse(_user, "take resName");
        var result = await _handler.Handle(cmd);
        
        Assert.True(result.TryPickT1(out var failure, out _));
        Assert.Equal("error", failure.Error);
    }
}