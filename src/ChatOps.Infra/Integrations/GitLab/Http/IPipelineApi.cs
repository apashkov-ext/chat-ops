using System.Text.Json.Serialization;
using FluentValidation;
using Refit;

namespace ChatOps.Infra.Integrations.GitLab.Http;

// TODO: retry
internal interface IPipelineApi
{
    [Post("/projects/{projectId}/pipeline")]
    Task<ApiResponse<CreatedPipelineDto>> Create(
        [AliasAs("projectId")] string projectId, 
        [Query] string @ref, 
        CancellationToken ct = default);
    
    [Post("/projects/{projectId}/pipeline")]
    Task<ApiResponse<CreatedPipelineDto>> Create(
        [AliasAs("projectId")] string projectId, 
        [Query] string @ref, 
        [Body(BodySerializationMethod.UrlEncoded)] IDictionary<string, string> variables,
        CancellationToken ct = default);
}

internal sealed class CreatedPipelineDto
{
    [JsonPropertyName("id")]
    public required int Id { get; init; }
    
    [JsonPropertyName("status")]
    public required string Status { get; init; }
    
    [JsonPropertyName("web_url")]
    public required string WebUrl { get; init; }
}

internal sealed class CreatedPipelineDtoValidator : AbstractValidator<CreatedPipelineDto>
{
    public CreatedPipelineDtoValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Status).NotEmpty();
    }
}