namespace ChatOps.Api.Extensions;

internal static class WebApplicationExtensions
{
    /// <summary>
    /// Настраивает мэппинг http-роутов. 
    /// </summary>
    /// <param name="app"></param>
    public static void MapFeatures(this WebApplication app)
    {
        
    }
}

internal static class EndpointRouteBuilderExtensions
{
    /// <summary>
    /// Регистрирует ендпоинт <typeparamref name="T"/> и его роутинги.
    /// </summary>
    /// <param name="endpoints"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IEndpointRouteBuilder MapEndpoint<T>(this IEndpointRouteBuilder endpoints) 
        where T : IEndpoint, new()
    {
        var endpoint = new T();
        endpoint.Map(endpoints);
        return endpoints;
    }
}

/// <summary>
/// Контракт для всех minimal-api ендпоинтов.
/// </summary>
internal interface IEndpoint
{
    void Map(IEndpointRouteBuilder endpointsBuilder);
}