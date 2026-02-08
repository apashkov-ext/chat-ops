using ChatOps.App.Core.Models;
using ChatOps.App.Features.Deploy;
using ChatOps.Infra.Integrations.GitLab;
using ChatOps.Infra.Integrations.GitLab.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OneOf;

namespace ChatOps.Infra.Features.Deploy;

internal sealed class GitLabCreatePipeline : ICreatePipeline
{
    private readonly GitLabOptions _options;
    private readonly IPipelineApi _pipelineApi;
    private readonly ILogger<GitLabCreatePipeline> _logger;

    public GitLabCreatePipeline(IOptions<GitLabOptions> gitLabOptions,
        IPipelineApi pipelineApi,
        ILogger<GitLabCreatePipeline> logger)
    {
        _options = gitLabOptions.Value;
        _pipelineApi = pipelineApi;
        _logger = logger;
    }
    
    public async Task<OneOf<CreatePipelineSuccess, CreatePipelineAlreadyExists, CreatePipelineFailure>> Execute(Resource resource, Ref @ref, CancellationToken ct = default)
    {
        var response = await _pipelineApi.Create(_options.Project.Value, @ref.Value, ct);
        
        
        // проверить, запущен ли уже пайплайн с такими же параметрами (если это возможно). Если запущен - что тогда? Варианты:
        // - остановить его и запустить новый
        // - запустить новый, не останавливая старый (поставить в очередь).
        // - сообщить юзеру и ничего не делать.
        
        throw new NotImplementedException();
    }
}