using System.ComponentModel.DataAnnotations;

namespace ChatOps.Api.Integrations.FileStorage;

internal sealed class ImagesOptions
{
    public const string SectionName = "Images";
    
    [Required]
    public required string Directory { get; init; }
}