using System.ComponentModel.DataAnnotations;

namespace ChatOps.Api.LocalAdapters.Files;

internal sealed class FilesOptions
{
    public const string SectionName = "Files";
    
    [Required]
    public required string Directory { get; init; }
}