namespace ChatOps.Api.Integrations.Telegram.Core;

internal sealed class CommandTokenCollection
{
    private const char _separator = ' ';
    private readonly string _stringRepresentation;

    public IReadOnlyList<string> Tokens { get; }
    public static CommandTokenCollection Empty { get; } = new ([]);

    private CommandTokenCollection(IEnumerable<string> tokens)
    {
        Tokens = [..tokens];
        _stringRepresentation = string.Join(_separator, [..Tokens]);
    }
    
    public static CommandTokenCollection Parse(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return new CommandTokenCollection([]);
        }

        var tokens = input.Split(_separator, 
            StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        return new CommandTokenCollection(tokens);
    }


    public override string ToString() => _stringRepresentation;
}