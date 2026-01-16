namespace ChatOps.App.Core.Models;

public sealed class BranchId : ValueObject
{
    public string Value { get; }

    public BranchId(string value)
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