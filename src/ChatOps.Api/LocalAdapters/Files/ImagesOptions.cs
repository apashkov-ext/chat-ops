using System.ComponentModel.DataAnnotations;

namespace ChatOps.Api.LocalAdapters.Files;

internal sealed class ImagesOptions
{
    public const string SectionName = "Images";
    
    [Required]
    public required string Directory { get; init; }
}