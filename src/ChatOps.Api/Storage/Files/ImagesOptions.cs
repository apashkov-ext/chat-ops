using System.ComponentModel.DataAnnotations;

namespace ChatOps.Api.Storage.Files;

internal sealed class ImagesOptions
{
    public const string SectionName = "Images";
    
    [Required]
    public required string Directory { get; init; }
}