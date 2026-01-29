using ChatOps.Infra.Integrations.GitLab.Http.Models;
using Refit;

namespace ChatOps.Infra.Integrations.GitLab.Http;

// TODO: вадидация ответов
// TODO: обработка ошибок
// TODO: retry
internal interface IPipelineApi
{
    [Post("/projects/{projectId}/pipeline?ref={ref}")]
    Task<ApiResponse<CreatedPipelineDto>> Create(string projectId, string @ref);
}