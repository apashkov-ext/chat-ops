using ChatOps.Api.Features.Deploy;
using ChatOps.Api.Integrations.Telegram.Core;
using ChatOps.Api.LocalAdapters.Users;
using ChatOps.App.Core.Models;
using ChatOps.App.Features.Deploy;
using Moq;
using Moq.AutoMock;

namespace ChatOps.Api.Tests;

public class DeployCommandHandlerTests
{
    private readonly TelegramUser _user;
    private readonly DeployCommandHandler _handler;
    private readonly Mock<IDeployUseCase> _deployUseCase;

    public DeployCommandHandlerTests()
    {
        _user = new TelegramUser(888, "user");
        
        var mocker = new AutoMocker();

        _deployUseCase = new Mock<IDeployUseCase>();
        _deployUseCase.Setup(x => x.Execute(
                It.IsAny<HolderId>(), 
                It.IsAny<ResourceId>(), 
                It.IsAny<RefName>(), 
                It.IsAny<IEnumerable<Variable>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DeploySuccess(new Pipeline(123, "https://link")));
        mocker.Use(_deployUseCase);

        var findTgUser = new Mock<IFindTelegramUserById>();
        mocker.Use(findTgUser);
        
        _handler = mocker.CreateInstance<DeployCommandHandler>();
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
    [InlineData("deploy")]
    [InlineData("deploy arg1")]
    [InlineData("deploy arg1 argN")]
    public void CanHandle_ShouldReturnTrue(string input)
    {
        var cmd = TelegramCommand.Parse(_user, input);
        var can = _handler.CanHandle(cmd);
        
        Assert.True(can);
    }

    [Fact]
    public async Task Handle_WithoutRequiredArg_ShouldNotInvokeUseCase()
    {
        var cmd = TelegramCommand.Parse(_user, "deploy");
        _ = await _handler.Handle(cmd);
        
        _deployUseCase.Verify(x => x.Execute(
            It.IsAny<HolderId>(),
            It.IsAny<ResourceId>(),
            It.IsAny<RefName>(),
            It.IsAny<IEnumerable<Variable>>(),
            It.IsAny<CancellationToken>()), Times.Never);
    }
    
    [Fact]
    public async Task Handle_WithoutRequiredArg_ShouldReturnFailure()
    {
        var cmd = TelegramCommand.Parse(_user, "deploy");
        var result = await _handler.Handle(cmd);
        
        Assert.True(result.TryPickT1(out var failure, out _));
        Assert.Equal("Invalid command syntax", failure.Error);
    }
    
    [Fact]
    public async Task Handle_ShouldInvokeTakeUseCase()
    {
        var cmd = TelegramCommand.Parse(_user, "deploy resourceName branchName");
        _ = await _handler.Handle(cmd);
        
        _deployUseCase.Verify(x => x.Execute(
            It.Is<HolderId>(h => h.Value == _user.Id.ToString()),
            It.Is<ResourceId>(r => r == new ResourceId("resourceName")),
            It.Is<RefName>(r => r == new RefName("branchName")),
            It.IsAny<IEnumerable<Variable>>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }    
    
    [Fact]
    public async Task Handle_Success_ShouldBeMappedToReply()
    {
        _deployUseCase.Setup(x => x.Execute(
                It.IsAny<HolderId>(), 
                It.IsAny<ResourceId>(), 
                It.IsAny<RefName>(),
                It.IsAny<IEnumerable<Variable>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DeploySuccess(new Pipeline(123, "https://link")));
        
        const string msg = """
                           ✅ Пайплайн <a href="https://link">#123</a> запущен.  
                           ресурс: resourceName
                           ветка/тег: branchName
                           """;
        
        var cmd = TelegramCommand.Parse(_user, "deploy resourceName branchName");
        var result = await _handler.Handle(cmd);
        
        Assert.True(result.TryPickT0(out var reply, out _));
        Assert.Equal(msg, reply.Text?.Text);
    }    
    
    [Fact]
    public async Task Handle_ResourceNotFound_ShouldBeMappedToReply()
    {
        _deployUseCase.Setup(x => x.Execute(
                It.IsAny<HolderId>(), 
                It.IsAny<ResourceId>(), 
                It.IsAny<RefName>(),
                It.IsAny<IEnumerable<Variable>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DeployResourceNotFound());
        
        var cmd = TelegramCommand.Parse(_user, "deploy resourceName branchName");
        var result = await _handler.Handle(cmd);
        
        Assert.True(result.TryPickT0(out var reply, out _));
        Assert.Equal("⚠️ Ресурс не найден", reply.Text?.Text);
    } 
    
    [Fact]
    public async Task Handle_ResourceNotReserved_ShouldBeMappedToReply()
    {
        _deployUseCase.Setup(x => x.Execute(
                It.IsAny<HolderId>(), 
                It.IsAny<ResourceId>(), 
                It.IsAny<RefName>(),
                It.IsAny<IEnumerable<Variable>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DeployResourceNotReserved());
        
        var cmd = TelegramCommand.Parse(_user, "deploy resourceName branchName");
        var result = await _handler.Handle(cmd);
        
        Assert.True(result.TryPickT0(out var reply, out _));
        Assert.Equal("⚠️ Сначала нужно зарезервировать этот ресурс", reply.Text?.Text);
    } 
    
    [Fact]
    public async Task Handle_RefNotFound_ShouldBeMappedToReply()
    {
        _deployUseCase.Setup(x => x.Execute(
                It.IsAny<HolderId>(), 
                It.IsAny<ResourceId>(), 
                It.IsAny<RefName>(),
                It.IsAny<IEnumerable<Variable>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DeployRefNotFound());
        
        var cmd = TelegramCommand.Parse(_user, "deploy resourceName branchName");
        var result = await _handler.Handle(cmd);
        
        Assert.True(result.TryPickT0(out var reply, out _));
        Assert.Equal("⚠️ Ветка/тег не найдена", reply.Text?.Text);
    }     
    
    [Fact]
    public async Task Handle_InProcess_ShouldBeMappedToReply()
    {
        _deployUseCase.Setup(x => x.Execute(
                It.IsAny<HolderId>(), 
                It.IsAny<ResourceId>(), 
                It.IsAny<RefName>(),
                It.IsAny<IEnumerable<Variable>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DeployInProcess(new Pipeline(123, "https://link")));
        
        const string msg = """
                            ℹ️ Пайплайн <a href="https://link">#123</a> уже запущен и выполняется.  
                            ресурс: resourceName
                            ветка/тег: branchName
                            """;
        
        var cmd = TelegramCommand.Parse(_user, "deploy resourceName branchName");
        var result = await _handler.Handle(cmd);
        
        Assert.True(result.TryPickT0(out var reply, out _));
        Assert.Equal(msg, reply.Text?.Text);
    }     
    
    [Fact]
    public async Task Handle_Failure_ShouldBeMappedToFailure()
    {
        _deployUseCase.Setup(x => x.Execute(
                It.IsAny<HolderId>(), 
                It.IsAny<ResourceId>(), 
                It.IsAny<RefName>(),
                It.IsAny<IEnumerable<Variable>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DeployFailure(DeployFailureReason.Unknown));
        
        const string msg = """
                           Не получилось запустить пайплайн.  
                           Пойдите и выясните причину.
                           """;
        
        var cmd = TelegramCommand.Parse(_user, "deploy resourceName branchName");
        var result = await _handler.Handle(cmd);
        
        Assert.True(result.TryPickT1(out var failure, out _));
        Assert.Equal(msg, failure.Error);
    } 
}