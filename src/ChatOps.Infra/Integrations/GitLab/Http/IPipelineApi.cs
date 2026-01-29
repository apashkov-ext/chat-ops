using Refit;

namespace ChatOps.Infra.Integrations.GitLab.Http;

// TODO: вадидация ответов
// TODO: обработка ошибок
// TODO: retry
internal interface IPipelineApi
{
    [Post("/projects/{projectId}/pipeline?ref={ref}")]
    [Headers("Content-Type: application/x-www-form-urlencoded")]
    Task<ApiResponse<CreatedPipelineDto>> Create(
        string projectId, 
        [AliasAs("ref")] string @ref, 
        CancellationToken ct = default);
}

internal sealed class CreatedPipelineDto
{
    public required int Id { get; init; }
    public required string Status { get; init; }
    public required string Ref { get; init; }
}