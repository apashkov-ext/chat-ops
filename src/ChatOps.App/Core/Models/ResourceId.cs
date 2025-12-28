namespace ChatOps.App.Core.Models;

public sealed class ResourceId : ValueObject
{
    public string Value { get; }

    public ResourceId(string value)
    {
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