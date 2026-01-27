using ChatOps.App.Core.Models;
using ChatOps.App.Features.Deploy;
using OneOf;

namespace ChatOps.Infra.Features.Deploy;

internal sealed class GitLabCreatePipeline : ICreatePipeline
{
    public Task<OneOf<CreatePipelineSuccess, CreatePipelineAlreadyExists, CreatePipelineFailure>> Execute(Resource resource, BranchId branch, CancellationToken ct = default)
    {
        // проверить, запущен ли уже пайплайн с такими же параметрами (если это возможно). Если запущен - что тогда? Варианты:
        // - остановить его и запустить новый
        // - запустить новый, не останавливая старый (поставить в очередь).
        // - сообщить юзеру и ничего не делать.
        
        throw new NotImplementedException();
    }
}