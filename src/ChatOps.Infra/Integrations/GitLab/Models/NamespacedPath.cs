using System.Text.RegularExpressions;
using ChatOps.App.Core;

namespace ChatOps.Infra.Integrations.GitLab.Models;

/// <summary>
/// https://docs.gitlab.com/api/rest/#namespaced-paths
/// </summary>
internal sealed class NamespacedPath : ValueObject
{
    private static readonly Regex _pattern = new (@"^[a-z](?:[a-z0-9_\.\-\/]*[a-z])$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    public string Value { get; }
    
    public NamespacedPath(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        
        var val = value.Trim();
        if (!_pattern.IsMatch(val))
        {
            throw new ArgumentException($"Invalid path: {val}");
        }
        
        Value = val;
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