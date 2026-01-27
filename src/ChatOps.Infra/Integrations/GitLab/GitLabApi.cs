using ChatOps.Infra.Integrations.GitLab.Models;
using Microsoft.Extensions.Options;

namespace ChatOps.Infra.Integrations.GitLab;

internal interface IGitLabApi
{
    Task<GitLabPipelineDto> CreatePipeline(Dictionary<string, string> variables,
        CancellationToken ct = default);
    
    Task<GitLabPipelineDto?> FindPipelineById(int id, 
        CancellationToken ct = default);
}

internal sealed class GitLabApi : IGitLabApi
{
    private readonly GitLabOptions _gitLabOptions;
    
    public GitLabApi(IOptions<GitLabOptions> gitLabOptions)
    {
        _gitLabOptions = gitLabOptions.Value;
    }

    public Task<GitLabPipelineDto> CreatePipeline(Dictionary<string, string> variables, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<GitLabPipelineDto?> FindPipelineById(int id, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}