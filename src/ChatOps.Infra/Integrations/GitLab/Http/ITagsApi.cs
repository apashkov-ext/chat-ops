using System.Text.Json.Serialization;
using FluentValidation;
using Refit;

namespace ChatOps.Infra.Integrations.GitLab.Http;

internal interface ITagsApi
{
    [Get("/projects/{projectId}/repository/tags/{tag}")]
    Task<ApiResponse<TagDto>> Single(
        [AliasAs("projectId")] string projectId, 
        [AliasAs("tag")] string tag, 
        CancellationToken ct = default);
}

internal sealed class TagDto
{
    [JsonPropertyName("name")]
    public required string Name { get; init; }
}

internal sealed class TagDtoValidator : AbstractValidator<TagDto>
{
    public TagDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}