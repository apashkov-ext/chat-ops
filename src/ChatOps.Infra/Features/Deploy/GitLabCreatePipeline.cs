using ChatOps.App.Core.Models;
using ChatOps.App.Features.Deploy;
using ChatOps.Infra.Integrations.GitLab.Http;
using OneOf;

namespace ChatOps.Infra.Features.Deploy;

internal sealed class GitLabCreatePipeline : ICreatePipeline
{
    private readonly IPipelineApi _pipelineApi;

    public GitLabCreatePipeline(IPipelineApi pipelineApi)
    {
        _pipelineApi = pipelineApi;
    }
    
    public Task<OneOf<CreatePipelineSuccess, CreatePipelineAlreadyExists, CreatePipelineFailure>> Execute(Resource resource, Ref @ref, CancellationToken ct = default)
    {
        // проверить, запущен ли уже пайплайн с такими же параметрами (если это возможно). Если запущен - что тогда? Варианты:
        // - остановить его и запустить новый
        // - запустить новый, не останавливая старый (поставить в очередь).
        // - сообщить юзеру и ничего не делать.
        
        throw new NotImplementedException();
    }
}