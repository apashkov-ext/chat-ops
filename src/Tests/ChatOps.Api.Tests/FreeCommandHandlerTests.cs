using ChatOps.Api.Features.Free;
using ChatOps.Api.Integrations.Telegram.Core;
using ChatOps.App.Core.Models;
using ChatOps.App.Features.Free;
using Moq;
using Moq.AutoMock;

namespace ChatOps.Api.Tests;

public class FreeCommandHandlerTests
{
    private readonly TelegramUser _user;
    private readonly FreeCommandHandler _handler;
    private readonly Mock<IFreeResourceUseCase> _freeResourceUseCase;
    private readonly Mock<IUsersCache> _cache;

    
    public FreeCommandHandlerTests()
    {
        _user = new TelegramUser(888, "user");
        
        var mocker = new AutoMocker();

        _freeResourceUseCase = new Mock<IFreeResourceUseCase>();
        _freeResourceUseCase.Setup(x => x.Execute(
                It.IsAny<HolderId>(), 
                It.IsAny<ResourceId>(), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FreeResourceSuccess());
        mocker.Use(_freeResourceUseCase);
        
        _cache = new Mock<IUsersCache>();
        mocker.Use(_cache);
        
        _handler = mocker.CreateInstance<FreeCommandHandler>();
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
        var cmd = TelegramCommand.Parse(_user, "ThisIsNotAFree3Token");
        var can = _handler.CanHandle(cmd);
        
        Assert.False(can);
    }
    
    [Theory]
    [InlineData("free")]
    [InlineData("free arg1")]
    [InlineData("free arg1 argN")]
    public void CanHandle_ShouldReturnTrue(string input)
    {
        var cmd = TelegramCommand.Parse(_user, input);
        var can = _handler.CanHandle(cmd);
        
        Assert.True(can);
    }

    [Fact]
    public async Task Handle_WithoutRequiredArg_ShouldNotInvokeUseCase()
    {
        var cmd = TelegramCommand.Parse(_user, "free");
        _ = await _handler.Handle(cmd);
        
        _freeResourceUseCase.Verify(x => x.Execute(
            It.IsAny<HolderId>(),
            It.IsAny<ResourceId>(),
            It.IsAny<CancellationToken>()), Times.Never);
    }
    
    [Fact]
    public async Task Handle_WithoutRequiredArg_ShouldReturnFailure()
    {
        var cmd = TelegramCommand.Parse(_user, "free");
        var result = await _handler.Handle(cmd);
        
        Assert.True(result.TryPickT1(out var failure, out _));
        Assert.Equal("Invalid command syntax", failure.Error);
    }
    
    [Fact]
    public async Task Handle_ShouldInvokeTakeUseCase()
    {
        var cmd = TelegramCommand.Parse(_user, "free resName");
        _ = await _handler.Handle(cmd);
        
        _freeResourceUseCase.Verify(x => x.Execute(
            It.Is<HolderId>(h => h.Value == _user.Id.ToString()),
            It.Is<ResourceId>(r => r == new ResourceId("resName")),
            It.IsAny<CancellationToken>()), Times.Once);
    }    
    
    [Fact]
    public async Task Handle_SuccessShouldBeMappedToReply()
    {
        _freeResourceUseCase.Setup(x => x.Execute(
                It.IsAny<HolderId>(), 
                It.IsAny<ResourceId>(), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FreeResourceSuccess());
        
        var cmd = TelegramCommand.Parse(_user, "free resName");
        var result = await _handler.Handle(cmd);
        
        Assert.True(result.TryPickT0(out var reply, out _));
        Assert.Equal("✅ Ресурс 'resName' освобожден", reply.Text?.Text);
    }       
    
    [Fact]
    public async Task Handle_NotFoundShouldBeMappedToReply()
    {
        _freeResourceUseCase.Setup(x => x.Execute(
                It.IsAny<HolderId>(), 
                It.IsAny<ResourceId>(), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FreeResourceNotFound());
        
        var cmd = TelegramCommand.Parse(_user, "free resName");
        var result = await _handler.Handle(cmd);
        
        Assert.True(result.TryPickT0(out var reply, out _));
        Assert.Equal("⚠️ Ресурс не найден", reply.Text?.Text);
    }     
    
    [Fact]
    public async Task Handle_ReservedAndUserNotCached_ShouldBeMappedToReply()
    {
        _freeResourceUseCase.Setup(x => x.Execute(
                It.IsAny<HolderId>(), 
                It.IsAny<ResourceId>(), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FreeResourceInUse(new HolderId("999")));
        
        var cmd = TelegramCommand.Parse(_user, "free resName");
        var result = await _handler.Handle(cmd);
        
        Assert.True(result.TryPickT0(out var reply, out _));
        Assert.Equal("⚠️ Ресурс зарезервирован для <a href=\"tg://user?id=999\">Личности без имени</a>", reply.Text?.Text);
    }        
    
    [Fact]
    public async Task Handle_ReservedAndUserCached_ShouldBeMappedToReply()
    {
        _freeResourceUseCase.Setup(x => x.Execute(
                It.IsAny<HolderId>(), 
                It.IsAny<ResourceId>(), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FreeResourceInUse(new HolderId("999")));
        
        _cache.Setup(x => x.Find(It.IsAny<long>())).Returns(new TelegramUser(999, "Имя", null, "username"));
        
        var cmd = TelegramCommand.Parse(_user, "free resName");
        var result = await _handler.Handle(cmd);
        
        Assert.True(result.TryPickT0(out var reply, out _));
        Assert.Equal("⚠️ Ресурс зарезервирован для <a href=\"tg://user?id=999\">@username</a>", reply.Text?.Text);
    }    
    
    [Fact]
    public async Task Handle_FailureShouldBeMappedToFailure()
    {
        _freeResourceUseCase.Setup(x => x.Execute(
                It.IsAny<HolderId>(), 
                It.IsAny<ResourceId>(), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FreeResourceFailure("error"));
        
        var cmd = TelegramCommand.Parse(_user, "free resName");
        var result = await _handler.Handle(cmd);
        
        Assert.True(result.TryPickT1(out var failure, out _));
        Assert.Equal("error", failure.Error);
    }
}