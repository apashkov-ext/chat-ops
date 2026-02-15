using ChatOps.Infra.Features.Deploy;
using ChatOps.Infra.Integrations.GitLab.Http;
using Moq;
using Moq.AutoMock;

namespace ChatOps.Infra.Tests;

public class GitLabCreatePipelineTests
{
    private readonly GitLabCreatePipeline _pipeline;
    private readonly Mock<IPipelineApi> _pipelineApi;
    
    public GitLabCreatePipelineTests()
    {
        var mocker = new AutoMocker();

        _pipelineApi = new Mock<IPipelineApi>();
        
        mocker.Use(_pipelineApi);
        
        _pipeline = mocker.CreateInstance<GitLabCreatePipeline>();
    }
    
    [Fact]
    public void Test1()
    {
        
    }
}