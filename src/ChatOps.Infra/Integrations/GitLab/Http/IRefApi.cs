using FluentValidation;
using Refit;

namespace ChatOps.Infra.Integrations.GitLab.Http;

// TODO: retry
internal interface IRefApi
{
    [Post("/projects/{projectId}/repository/branches/{ref}")]
    Task<ApiResponse<RefDto>> Single(string projectId, string @ref, CancellationToken ct = default);
}

internal sealed class RefDto
{
    public required string Name { get; init; }
}

internal sealed class RefDtoValidator : AbstractValidator<RefDto>
{
    public RefDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}