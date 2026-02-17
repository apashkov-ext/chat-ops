using System.ComponentModel.DataAnnotations;

namespace ChatOps.Infra.Integrations.GitLab.Models;

internal sealed class GitLabPipelineDto
{
    [Required]
    public required int Id { get; init; }
    
    [Required]
    public required string Status { get; init; }
    
    [Required]
    public required string Ref { get; init; }
}