using Microsoft.Extensions.Options;

namespace ChatOps.Api.Integrations.FileStorage;

internal interface IImageResolver
{
    Stream ResolveById(string imageId);
}

internal sealed class ImageResolver : IImageResolver
{
    private readonly ImagesOptions _options;
    
    public ImageResolver(IOptions<ImagesOptions> imagesOptions)
    {
        _options = imagesOptions.Value;
    }
    
    public Stream ResolveById(string imageId)
    {
        var path = Path.Combine(_options.Directory, imageId);
        
        try
        {
            return File.OpenRead(path);
        }
        catch (Exception e)
        {
            throw new Exception("Failed to read image content", e);
        }
    }
}