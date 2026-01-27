using System.Reflection;

namespace ChatOps.Api.Features.Start;

internal interface IApplicationVersionResolver
{
    string GetVersion();
}

internal sealed class ApplicationVersionResolver : IApplicationVersionResolver
{
    private static readonly Lazy<string> _version = new(() =>
    {
        var attribute = Assembly.GetExecutingAssembly()
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
            .InformationalVersion;

        return attribute
               ?? Assembly.GetEntryAssembly()?.GetName().Version?.ToString()
               ?? "unknown";
    });
    
    public string GetVersion()
    {
        return _version.Value;
    }
}