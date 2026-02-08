using System.Text.RegularExpressions;

namespace ChatOps.App.Core.Models;

public sealed class VariableName : ValueObject
{
    private static readonly Regex _pattern = new ("^[a-z](?:[a-z_]*[a-z])$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    
    public string Value { get; }

    public VariableName(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        
        var val = value.Trim();
        if (!_pattern.IsMatch(val))
        {
            throw new ArgumentException($"Invalid argument name: {val}");
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
    
    public static implicit operator VariableName(string value) => new (value);
}