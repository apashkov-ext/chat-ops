using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChatOps.Infra.Integrations.GitLab;

public static class Module
{
    public static void AddGitLabIntegration(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddOptionsWithValidateOnStart<GitLabOptions>()
            .BindConfiguration(GitLabOptions.SectionName)
            .ValidateDataAnnotations();
       
        services.AddTransient<IGitLabApi, GitLabApi>();
    }    
}