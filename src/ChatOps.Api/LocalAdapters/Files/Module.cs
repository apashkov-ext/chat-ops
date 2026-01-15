namespace ChatOps.Api.LocalAdapters.Files;

internal static class Module
{
    public static void AddFilesDatabase(this WebApplicationBuilder builder)
    {
        builder.Services.AddOptionsWithValidateOnStart<FilesOptions>()
            .BindConfiguration(FilesOptions.SectionName)
            .ValidateDataAnnotations();
        
        builder.Services.AddTransient<IGetStreamByFileId, GetStreamByFileId>();
    }
}