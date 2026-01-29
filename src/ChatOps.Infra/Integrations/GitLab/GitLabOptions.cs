using System.ComponentModel.DataAnnotations;
using ChatOps.Infra.Integrations.GitLab.Models;

namespace ChatOps.Infra.Integrations.GitLab;

internal sealed class GitLabOptions
{
    public const string SectionName = "GitLab";
    
    [Required]
    public required string Host { get; init; }
    
    [Required]
    public required string Token { get; init; }
    
    [Required]
    public required string ProjectId { get; init; }
    
    public Uri HostUri => new(Host.TrimEnd('/'));
    public GitLabProjectId Project => new (ProjectId);
}