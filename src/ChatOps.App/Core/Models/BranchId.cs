namespace ChatOps.App.Core.Models;

public sealed class BranchId : ValueObject
{
    public string Value { get; }
    public string UrlEncodedValue { get; }

    public BranchId(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        Value = value;
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
}
