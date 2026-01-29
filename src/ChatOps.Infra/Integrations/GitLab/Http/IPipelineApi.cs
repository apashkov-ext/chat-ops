using ChatOps.App.Core.Models;
using ChatOps.Infra.Integrations.GitLab.Http.Models;
using ChatOps.Infra.Integrations.GitLab.Models;
using Refit;

namespace ChatOps.Infra.Integrations.GitLab.Http;

// TODO: вадидация ответов
// TODO: обработка ошибок
// TODO: retry
internal interface IPipelineApi
{
    [Post("/projects/{projectId}/pipeline?ref={branchId}")]
    Task<CreatedPipelineDto> Create(GitLabProjectId projectId, BranchId branchId);
}