using FluentValidation;
using Refit;

namespace ChatOps.Infra.Integrations.GitLab.Http;

// TODO: вадидация ответов
// TODO: обработка ошибок
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
    public required int Id { get; init; }
    public required string Status { get; init; }
    public required string Ref { get; init; }
}

internal sealed class CreatedPipelineDtoValidator : AbstractValidator<CreatedPipelineDto>
{
    public CreatedPipelineDtoValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Status).NotEmpty();
        RuleFor(x => x.Ref).NotEmpty();
    }
}