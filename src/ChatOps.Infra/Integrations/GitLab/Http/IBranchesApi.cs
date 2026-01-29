using ChatOps.Infra.Integrations.GitLab.Http.Models;
using Refit;

namespace ChatOps.Infra.Integrations.GitLab.Http;

internal interface IBranchesApi
{
    [Post("/projects/{projectId}/repository/branches/{ref}")]
    Task<ApiResponse<BranchDto>> Single(string projectId, string @ref);
}