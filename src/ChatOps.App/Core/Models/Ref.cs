namespace ChatOps.App.Core.Models;

/// <summary>
/// Ветка или тег.
/// </summary>
public sealed class Ref : ValueObject
{
    public string Value { get; }

    public Ref(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        Value = value;
    }
    
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    protected override string GetStringRepresentation()
    {
        return Value;
    }
}
