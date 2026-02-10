namespace ChatOps.App.Core.Models;

public sealed class PipelineLink : ValueObject
{
    public string Value { get; }

    public PipelineLink(string value)
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
    
    public static implicit operator PipelineLink(string link) => new (link);
}