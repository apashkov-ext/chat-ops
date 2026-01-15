namespace ChatOps.Api.Storage.Files;

internal static class Module
{
    public static void AddImageDatabase(this IServiceCollection services)
    {
        services.AddOptionsWithValidateOnStart<ImagesOptions>()
            .BindConfiguration(ImagesOptions.SectionName)
            .ValidateDataAnnotations();
        services.AddTransient<IImageResolver, ImageResolver>();
    }
}