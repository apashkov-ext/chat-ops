using Microsoft.Extensions.Options;

namespace ChatOps.Api.LocalAdapters.Files;

internal interface IGetStreamByFileId
{
    Stream Execute(string imageId);
}

internal sealed class GetStreamByFileId : IGetStreamByFileId
{
    private readonly FilesOptions _options;
    
    public GetStreamByFileId(IOptions<FilesOptions> imagesOptions)
    {
        _options = imagesOptions.Value;
    }
    
    public Stream Execute(string imageId)
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