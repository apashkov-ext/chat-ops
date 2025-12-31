using ChatOps.App.Ports;
using ChatOps.Infra.Adapters.InMemory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChatOps.Infra.Extensions;

public static class ServiceCollectionExtensions
{
    public static void RegisterInfrastructureServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddSingleton<IGetResources, InMemoryGetResources>();
    }
}