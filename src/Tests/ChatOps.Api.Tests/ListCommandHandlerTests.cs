using ChatOps.Api.Features.List;
using ChatOps.Api.Integrations.Telegram.Core;
using ChatOps.App.Core.Models;
using ChatOps.App.UseCases.ListResources;
using Moq;
using Moq.AutoMock;

namespace ChatOps.Api.Tests;

public class ListCommandHandlerTests
{
    private readonly ListCommandHandler _handler;
    private readonly Mock<IListResourcesUseCase> _listResourcesUseCase;
    
    public ListCommandHandlerTests()
    {
        var mocker = new AutoMocker();

        _listResourcesUseCase = new Mock<IListResourcesUseCase>();
        _listResourcesUseCase.Setup(x => x.Execute(It.IsAny<CancellationToken>())).ReturnsAsync([]);
        mocker.Use(_listResourcesUseCase);
        
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
        
        var result = await _handler.Handle(CommandTokenCollection.Empty);
        
        Assert.True(result.TryPickT0(out var reply, out _));
        Assert.Equal(expectedMessage, reply.Text);
    }    
    
    [Fact]
    public async Task ShouldReturnNonEmptyList()
    {
        var expectedMessage =
                """
                <b>Список ресурсов</b>

                 1. dev, свободен
                 2. dev1, занят @user
                 3. dev2, свободен
                """
            ;
        
        Resource[] model = 
        [
            new Resource
            {
                Id = new ResourceId(Guid.NewGuid().ToString()),
                Name = "dev",
                State = ResourceState.Free
            },
            new Resource
            {
                Id = new ResourceId(Guid.NewGuid().ToString()),
                Name = "dev1",
                State = ResourceState.Reserved,
                Holder = "@user"
            },
            new Resource
            {
                Id = new ResourceId(Guid.NewGuid().ToString()),
                Name = "dev2",
                State = ResourceState.Free
            }
        ];
        _listResourcesUseCase.Setup(x => x.Execute(It.IsAny<CancellationToken>())).ReturnsAsync(model);
        
        var result = await _handler.Handle(CommandTokenCollection.Empty);

        Assert.True(result.TryPickT0(out var reply, out _));
        Assert.Equal(expectedMessage, reply.Text);
    }
}