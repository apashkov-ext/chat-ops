namespace ChatOps.App.Core.Models;

public sealed class RefName : ValueObject
{
    public string Value { get; }
    
    public RefName(string value)
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
    
    public static implicit operator RefName(string name) => new (name);
}