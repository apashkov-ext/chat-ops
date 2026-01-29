using Refit;

namespace ChatOps.Infra.Integrations.GitLab.Http;

internal interface IBranchesApi
{
    [Post("/projects/{projectId}/repository/branches/{ref}")]
    Task<ApiResponse<BranchDto>> Single(string projectId, string @ref, CancellationToken ct = default);
}

internal sealed class BranchDto
{
    public required string Name { get; init; }
}