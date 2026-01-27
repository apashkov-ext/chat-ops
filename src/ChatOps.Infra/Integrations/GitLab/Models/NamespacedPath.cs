using ChatOps.App.Core;

namespace ChatOps.Infra.Integrations.GitLab.Models;

/// <summary>
/// https://docs.gitlab.com/api/rest/#namespaced-paths
/// </summary>
internal sealed class NamespacedPath : ValueObject
{
    public string Value { get; }
    public string UrlEncodedValue { get; }
    
    public NamespacedPath(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        Value = value.Trim();
        UrlEncodedValue = Uri.EscapeDataString(Value);
    }
    
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    protected override string GetStringRepresentation()
    {
        return Value;
    }
    
    public static implicit operator NamespacedPath(string value) => new (value);
}