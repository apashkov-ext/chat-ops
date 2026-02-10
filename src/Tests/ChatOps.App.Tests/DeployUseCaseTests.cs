using ChatOps.App.Features.Deploy;
using ChatOps.App.SharedPorts;
using Moq;
using Moq.AutoMock;

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
        mocker.Use(_createPipeline);

        _useCase = mocker.CreateInstance<DeployUseCase>();
    }
}