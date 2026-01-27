using ChatOps.App.Core.Models;
using ChatOps.App.Features.Free;
using ChatOps.App.SharedPorts;
using Moq;
using Moq.AutoMock;

namespace ChatOps.App.Tests;

public class FreeResourceUseCaseTests
{
    private readonly FreeResourceUseCase _useCase;
    private readonly Mock<IFindResourceById> _findResourceById;
    private readonly Mock<IUpdateResource> _updateResource;
    
    public FreeResourceUseCaseTests()
    {
        var mocker = new AutoMocker();

        _findResourceById = new Mock<IFindResourceById>();
        mocker.Use(_findResourceById);
        
        _updateResource = new Mock<IUpdateResource>();
        mocker.Use(_updateResource);
        
        _useCase = mocker.CreateInstance<FreeResourceUseCase>();
    }

    [Fact]
    public async Task ResourceNotFound_ShouldReturnNotFound()
    {
        var holderId = new HolderId("888");
        var resourceId = new ResourceId("id");
        
        var result = await _useCase.Execute(holderId, resourceId);
        
        Assert.True(result.TryPickT1(out _, out _));
    }
    
    [Fact]
    public async Task AlreadyFree_ShouldReturnAlreadyFree()
    {
        var holderId = new HolderId("888");
        var resourceId = new ResourceId("id");
        _findResourceById.Setup(x => x.Execute(It.IsAny<ResourceId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Resource(resourceId, ResourceState.Free, null));
        
        var result = await _useCase.Execute(holderId, resourceId);
        
        Assert.True(result.TryPickT3(out _, out _));
    }

    [Fact]
    public async Task InUseByAnotherUser_ShouldReturnInUse()
    {
        var holderId = new HolderId("888");
        var resourceId = new ResourceId("id");
        _findResourceById.Setup(x => x.Execute(It.IsAny<ResourceId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Resource(resourceId, ResourceState.Reserved, new HolderId("999")));
        
        var result = await _useCase.Execute(holderId, resourceId);
        
        Assert.True(result.TryPickT2(out var inUse, out _));
        Assert.Equal(new HolderId("999"), inUse.HolderId);
    }    
    
    [Fact]
    public async Task ShouldReturnSuccess()
    {
        var holderId = new HolderId("888");
        var resourceId = new ResourceId("id");
        _findResourceById.Setup(x => x.Execute(It.IsAny<ResourceId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Resource(resourceId, ResourceState.Reserved, new HolderId("888")));
        
        var result = await _useCase.Execute(holderId, resourceId);
        
        Assert.True(result.TryPickT0(out _, out _));
    }   
    
    [Fact]
    public async Task ShouldInvokeUpdate()
    {
        var holderId = new HolderId("888");
        var resourceId = new ResourceId("id");
        _findResourceById.Setup(x => x.Execute(It.IsAny<ResourceId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Resource(resourceId, ResourceState.Reserved, new HolderId("888")));
        
        _ = await _useCase.Execute(holderId, resourceId);
        
        _updateResource.Verify(x => x.Execute(It.Is<Resource>(r => r.Id == resourceId), It.IsAny<CancellationToken>()), Times.Once);
    }   
}