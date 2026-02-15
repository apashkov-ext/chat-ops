using System.Reflection;
using ChatOps.Infra.Integrations.GitLab.Http;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Refit;

namespace ChatOps.Infra.Integrations.GitLab;

public static class Module
{
    public static void AddGitLabIntegration(this IServiceCollection services, ConfigurationManager _)
    {
        services.AddOptionsWithValidateOnStart<GitLabOptions>()
            .BindConfiguration(GitLabOptions.SectionName)
            .ValidateDataAnnotations();

        services.AddValidatorsFromAssembly(
            Assembly.GetExecutingAssembly(), 
            ServiceLifetime.Transient,
            includeInternalTypes: true);
        
        services.AddGitLabRefit<IPipelineApi>();
        services.AddGitLabRefit<IRefApi>();
    }    
    
    private static void AddGitLabRefit<T>(this IServiceCollection services) where T : class
    {
        services.AddTransient<GitLabAuthHandler>();

        services.AddRefitClient<T>()
            .ConfigureHttpClient((sp, c) =>
            {
                var options = sp.GetRequiredService<IOptions<GitLabOptions>>().Value;
                c.BaseAddress = options.HostUri;
            })
            .AddHttpMessageHandler<GitLabAuthHandler>();
    }
}