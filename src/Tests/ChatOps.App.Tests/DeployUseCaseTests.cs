using ChatOps.App.Core.Models;
using ChatOps.App.Features.Deploy;
using ChatOps.App.SharedPorts;
using Moq;
using Moq.AutoMock;
using CreatePipelineResult = OneOf.OneOf<
    ChatOps.App.Features.Deploy.CreatePipelineSuccess,
    ChatOps.App.Features.Deploy.CreatePipelineFailure
>;

namespace ChatOps.App.Tests;

public class DeployUseCaseTests
{
    private readonly Mock<IFindResourceById> _findResourceById;
    private readonly Mock<IFindRef> _findRef;
    private readonly Mock<ICreatePipeline> _createPipeline;

    private readonly DeployUseCase _useCase;

    public DeployUseCaseTests()
    {
        var mocker = new AutoMocker();

        _findResourceById = new Mock<IFindResourceById>();
        mocker.Use(_findResourceById);

        _findRef = new Mock<IFindRef>();
        mocker.Use(_findRef);

        _createPipeline = new Mock<ICreatePipeline>();
        _createPipeline.SetupWithAny<ICreatePipeline, Task<CreatePipelineResult>>(nameof(ICreatePipeline.Execute))
            .ReturnsAsync(new CreatePipelineSuccess(new Pipeline(123, "https://link")));
        mocker.Use(_createPipeline);

        _useCase = mocker.CreateInstance<DeployUseCase>();
    }
    
    [Fact]
    public async Task ResourceNotFound_ShouldReturnNotFound()
    {
        var holderId = new HolderId("888");
        var resourceId = new ResourceId("id");
        var @ref = new Ref("master");
        
        var result = await _useCase.Execute(holderId, resourceId, @ref, []);
        
        Assert.True(result.TryPickT1(out _, out _));
    }
    
    [Fact]
    public async Task InUseByAnotherUser_ShouldReturnNotReserved()
    {
        var holderId = new HolderId("888");
        var resourceId = new ResourceId("id");
        var @ref = new Ref("master");
        _findResourceById.Setup(x => x.Execute(It.IsAny<ResourceId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Resource(resourceId, ResourceState.Reserved, new HolderId("999")));
        
        var result = await _useCase.Execute(holderId, resourceId, @ref, []);
        
        Assert.True(result.TryPickT2(out _, out _));
    }  
    
    [Fact]
    public async Task IsNotReserved_ShouldReturnNotReserved()
    {
        var holderId = new HolderId("888");
        var resourceId = new ResourceId("id");
        var @ref = new Ref("master");
        _findResourceById.Setup(x => x.Execute(It.IsAny<ResourceId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Resource(resourceId, ResourceState.Free, null));
        
        var result = await _useCase.Execute(holderId, resourceId, @ref, []);
        
        Assert.True(result.TryPickT2(out _, out _));
    }  
    
    [Fact]
    public async Task RefNotFound_ShouldReturnRefNotFound()
    {
        var holderId = new HolderId("888");
        var resourceId = new ResourceId("id");
        var @ref = new Ref("master");
        var resource = new Resource(resourceId, ResourceState.Reserved, new HolderId("888"));
        _findResourceById.Setup(x => x.Execute(It.IsAny<ResourceId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(resource);
        
        var result = await _useCase.Execute(holderId, resourceId, @ref, []);
        
        Assert.True(result.TryPickT3(out _, out _));
    }     
    
    [Fact]
    public async Task ShouldInvokeCreatePipeline()
    {
        var holderId = new HolderId("888");
        var resourceId = new ResourceId("id");
        var @ref = new Ref("branch");
        var resource = new Resource(resourceId, ResourceState.Reserved, new HolderId("888"));
        _findResourceById.Setup(x => x.Execute(It.IsAny<ResourceId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(resource);
        _findRef.Setup(x => x.Execute(It.IsAny<Ref>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Ref("branch"));
        
        await _useCase.Execute(holderId, resourceId, @ref, []);
        
        _createPipeline.Verify(x => x.Execute(
            It.Is<Resource>(r => r.Id == new ResourceId("id")), 
            It.Is<Ref>(r => r == new Ref("branch")), 
            It.IsAny<IEnumerable<Variable>>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }  
    
    [Fact]
    public async Task CreatePipelineSuccess_ShouldMappedToSuccess()
    {
        var holderId = new HolderId("888");
        var resourceId = new ResourceId("id");
        var @ref = new Ref("branch");
        var resource = new Resource(resourceId, ResourceState.Reserved, new HolderId("888"));
        _findResourceById.Setup(x => x.Execute(It.IsAny<ResourceId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(resource);
        _findRef.SetupWithAny<IFindRef, Task<Ref>>(nameof(IFindRef.Execute)).ReturnsAsync(new Ref("branch"));
        
        var result = await _useCase.Execute(holderId, resourceId, @ref, []);
        
        Assert.True(result.TryPickT0(out var success, out _));
        Assert.Equal(new Pipeline(123, "https://link"), success.Pipeline);
    }      
    
    [Fact]
    public async Task CreatePipelineFailure_ShouldMappedToFailure()
    {
        var holderId = new HolderId("888");
        var resourceId = new ResourceId("id");
        var @ref = new Ref("branch");
        var resource = new Resource(resourceId, ResourceState.Reserved, new HolderId("888"));
        _findResourceById.Setup(x => x.Execute(It.IsAny<ResourceId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(resource);
        _findRef.SetupWithAny<IFindRef, Task<Ref>>(nameof(IFindRef.Execute)).ReturnsAsync(new Ref("branch"));
        _createPipeline.SetupWithAny<ICreatePipeline, Task<CreatePipelineResult>>(nameof(ICreatePipeline.Execute))
            .ReturnsAsync(new CreatePipelineFailure(CreatePipelineFailureReason.Unknown));
        
        var result = await _useCase.Execute(holderId, resourceId, @ref, []);
        
        Assert.True(result.TryPickT5(out _, out _));
    }  
}