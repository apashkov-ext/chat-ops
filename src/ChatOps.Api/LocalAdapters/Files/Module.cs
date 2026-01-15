namespace ChatOps.Api.LocalAdapters.Files;

internal static class Module
{
    public static void AddImagesDatabase(this WebApplicationBuilder builder)
    {
        builder.Services.AddOptionsWithValidateOnStart<ImagesOptions>()
            .BindConfiguration(ImagesOptions.SectionName)
            .ValidateDataAnnotations();
        
        builder.Services.AddTransient<IImageResolver, ImageResolver>();
    }
}