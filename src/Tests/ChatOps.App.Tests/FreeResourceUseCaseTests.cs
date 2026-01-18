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
    public void ResourceNotFound_ShouldReturnNotFound()
    {
        throw new NotImplementedException();
    }
}