using System.Collections.Immutable;

namespace ChatOps.Api.Integrations.Telegram.Handling;

internal sealed class CommandTokenCollection
{
    public ImmutableArray<string> Tokens { get; }
    
    public bool Empty => Tokens.IsEmpty;

    private CommandTokenCollection(IEnumerable<string> tokens)
    {
        Tokens = [..tokens];
    }
    
    public static CommandTokenCollection Parse(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return new CommandTokenCollection([]);
        }

        var tokens = input.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        return new CommandTokenCollection(tokens);
    }
}