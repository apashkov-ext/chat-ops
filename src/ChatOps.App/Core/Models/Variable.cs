namespace ChatOps.App.Core.Models;

public sealed class Variable : ValueObject
{
    public string Name { get; }
    public string Value { get; }

    public Variable(VariableName name, string value)
    {
        Name = name.Value;
        Value = value;
    }
    
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Name;
        yield return Value;
    }

    protected override string GetStringRepresentation()
    {
        return $"{Name}={Value}";
    }
}