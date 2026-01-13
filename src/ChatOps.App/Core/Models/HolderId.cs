namespace ChatOps.App.Core.Models;

public sealed class HolderId : ValueObject
{
    public string Value { get; }

    public HolderId(string value)
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