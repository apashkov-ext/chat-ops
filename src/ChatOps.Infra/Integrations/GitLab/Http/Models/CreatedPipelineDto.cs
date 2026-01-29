namespace ChatOps.Infra.Integrations.GitLab.Http.Models;

internal sealed class CreatedPipelineDto
{
    public required int Id { get; init; }
    public required string Status { get; init; }
    public required string Ref { get; init; }
}