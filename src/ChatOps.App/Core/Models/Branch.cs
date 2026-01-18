namespace ChatOps.App.Core.Models;

public sealed class Branch : ValueObject
{
    public string Value { get; }

    public Branch(string value)
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

    public string GetUrlEncodedValue()
    {
        return Uri.EscapeDataString(Value);
    }
}
