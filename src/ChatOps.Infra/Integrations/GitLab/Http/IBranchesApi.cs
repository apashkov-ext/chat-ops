using System.Text.Json.Serialization;
using FluentValidation;
using Refit;

namespace ChatOps.Infra.Integrations.GitLab.Http;

// TODO: retry
internal interface IBranchesApi
{
    [Get("/projects/{projectId}/repository/branches/{branch}")]
    Task<ApiResponse<BranchDto>> Single(
        [AliasAs("projectId")] string projectId, 
        [AliasAs("branch")] string branch, 
        CancellationToken ct = default);
}

internal sealed class BranchDto
{
    [JsonPropertyName("name")]
    public required string Name { get; init; }
}

internal sealed class BranchDtoValidator : AbstractValidator<BranchDto>
{
    public BranchDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}
