using ChatOps.Api.Features.List;
using ChatOps.Api.Integrations.Telegram.Core;
using ChatOps.Api.Storage.Users;
using ChatOps.App.Core.Models;
using ChatOps.App.Features.List;
using Moq;
using Moq.AutoMock;

namespace ChatOps.Api.Tests;

public class ListCommandHandlerTests
{
    private readonly ListCommandHandler _handler;
    private readonly Mock<IListResourcesUseCase> _listResourcesUseCase;
    private readonly Mock<IUsersCache> _usersCache;
    
    public ListCommandHandlerTests()
    {
        var mocker = new AutoMocker();

        _listResourcesUseCase = new Mock<IListResourcesUseCase>();
        _listResourcesUseCase.Setup(x => x.Execute(It.IsAny<CancellationToken>())).ReturnsAsync([]);
        mocker.Use(_listResourcesUseCase);
        
        _usersCache = new Mock<IUsersCache>();
        mocker.Use(_usersCache);
        
        _handler = mocker.CreateInstance<ListCommandHandler>();
    }
    
    [Fact]
    public async Task ShouldReturnEmptyList()
    {
        var expectedMessage =
                $"""
                 <b>Список ресурсов</b>

                 [пусто]
                 """
            ;
        
        var result = await _handler.Handle(TelegramCommand.Empty(new TelegramUser(888, "user")));
        
        Assert.True(result.TryPickT0(out var reply, out _));
        Assert.Equal(expectedMessage, reply.Text?.Text);
    }    
    
    [Fact]
    public async Task ShouldReturnNonEmptyList()
    {
        const string expectedMessage = """
                                       <b>Список ресурсов</b>

                                        1. dev, свободен
                                        2. dev1, занят <a href="tg://user?id=888">@user</a>
                                        3. dev2, свободен
                                       """;
        
        Resource[] model = 
        [
            new(new ResourceId("dev"), ResourceState.Free, null),
            new(new ResourceId("dev1"), ResourceState.Reserved, new HolderId("888")),
            new(new ResourceId("dev2"), ResourceState.Free, null)
        ];
        _listResourcesUseCase.Setup(x => x.Execute(It.IsAny<CancellationToken>())).ReturnsAsync(model);
        _usersCache.Setup(x => x.Find(It.Is<long>(m => m == 888)))
            .Returns(new TelegramUser(888, "Имя", null, "user"));
        
        var result = await _handler.Handle(TelegramCommand.Empty(new TelegramUser(999, "Имя", null, "user2")));

        Assert.True(result.TryPickT0(out var reply, out _));
        Assert.Equal(expectedMessage, reply.Text?.Text);
    }
}