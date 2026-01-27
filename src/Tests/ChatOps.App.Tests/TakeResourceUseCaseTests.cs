using ChatOps.App.Core.Models;
using ChatOps.App.Features.Take;
using ChatOps.App.SharedPorts;
using Moq;
using Moq.AutoMock;

namespace ChatOps.App.Tests;

public class TakeResourceUseCaseTests
{
    private readonly Mock<IFindResourceById> _findResourceById;
    private readonly Mock<IUpdateResource> _updateResource;
    private readonly Mock<ICountReservedResourcesByHolder> _countResources;
    
    private readonly TakeResourceUseCase _useCase;
    
    public TakeResourceUseCaseTests()
    {
        var mocker = new AutoMocker();
        
        _findResourceById = new Mock<IFindResourceById>();
        mocker.Use(_findResourceById);
        
        _updateResource = new Mock<IUpdateResource>();
        mocker.Use(_updateResource);
        
        _countResources  = new Mock<ICountReservedResourcesByHolder>();
        mocker.Use(_countResources);
        
        _useCase = mocker.CreateInstance<TakeResourceUseCase>();
    }
    
    [Fact]
    public async Task ResourceNotFound_ShouldReturnNotFound()
    {
        var holderId = new HolderId("888");
        var resourceId = new ResourceId("id");
        
        var result = await _useCase.Execute(holderId, resourceId);
        
        Assert.True(result.TryPickT1(out _, out _));
    }

    [Theory]
    // Достигнут максимум
    [InlineData(TakeResourceUseCase.MaxResourcesPerUser)]
    // Больше максимума
    [InlineData(TakeResourceUseCase.MaxResourcesPerUser + 1)]
    public async Task TooManyResources_ShouldReturnLimitExceeded(int count)
    {
        var holderId = new HolderId("888");
        var resourceId = new ResourceId("id");
        _findResourceById.Setup(x => x.Execute(It.IsAny<ResourceId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Resource(resourceId, ResourceState.Free, null));
        _countResources.Setup(x => x.Execute(
                It.IsAny<HolderId>(), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(count);
        
        var result = await _useCase.Execute(holderId, resourceId);
        
        Assert.True(result.TryPickT2(out _, out _));
    }
    
    [Fact]
    public async Task InUseByAnotherUser_ShouldReturnInUse()
    {
        var holderId = new HolderId("888");
        var resourceId = new ResourceId("id");
        _findResourceById.Setup(x => x.Execute(It.IsAny<ResourceId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Resource(resourceId, ResourceState.Reserved, new HolderId("999")));
        
        var result = await _useCase.Execute(holderId, resourceId);
        
        Assert.True(result.TryPickT3(out var inUse, out _));
        Assert.Equal(new HolderId("999"), inUse.HolderId);
    }  
    
    [Fact]
    public async Task IsFree_ShouldReturnSuccess()
    {
        var holderId = new HolderId("888");
        var resourceId = new ResourceId("id");
        _findResourceById.Setup(x => x.Execute(It.IsAny<ResourceId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Resource(resourceId, ResourceState.Free, null));
        _countResources.Setup(x => x.Execute(
                It.IsAny<HolderId>(), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);
        
        var result = await _useCase.Execute(holderId, resourceId);
        
        Assert.True(result.TryPickT0(out _, out _));
    } 
    
    [Fact]
    public async Task IsReservedButHolderIsNull_ShouldReturnSuccess()
    {
        var holderId = new HolderId("888");
        var resourceId = new ResourceId("id");
        _findResourceById.Setup(x => x.Execute(It.IsAny<ResourceId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Resource(resourceId, ResourceState.Reserved, null));
        _countResources.Setup(x => x.Execute(
                It.IsAny<HolderId>(), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);
        
        var result = await _useCase.Execute(holderId, resourceId);
        
        Assert.True(result.TryPickT0(out _, out _));
    } 
    
    [Fact]
    public async Task ShouldInvokeUpdate()
    {
        var holderId = new HolderId("888");
        var resourceId = new ResourceId("id");
        _findResourceById.Setup(x => x.Execute(It.IsAny<ResourceId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Resource(resourceId, ResourceState.Free, null));
        _countResources.Setup(x => x.Execute(
                It.IsAny<HolderId>(), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);
        
        _ = await _useCase.Execute(holderId, resourceId);
        
        _updateResource.Verify(x => x.Execute(It.Is<Resource>(r => r.Id == resourceId), It.IsAny<CancellationToken>()), Times.Once);
    }   
}