namespace ChatOps.Api.Features.TelegramMessageHandler.Handling;

internal readonly struct CommandWord
{
    public string Value  { get; }

    public CommandWord(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        Value = value;
    }
}

internal readonly struct CommandArgument
{
    public string Value { get; }
    
    public CommandArgument(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        Value = value;
    }
}

internal sealed class CommandTokenCollection
{
    private const char _separator = ' ';
    private readonly string _stringRepresentation;

    private readonly string[] _tokens;
    
    public bool Empty => _tokens.Length == 0;

    private CommandTokenCollection(IEnumerable<string> tokens)
    {
        _tokens = [..tokens];
        _stringRepresentation = string.Join(_separator, _tokens);
    }
    
    public static CommandTokenCollection Parse(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return new CommandTokenCollection([]);
        }

        var tokens = input.Split(_separator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        
        return new CommandTokenCollection(tokens);
    }

    public CommandBuffer GetBuffer() => new (_tokens);

    public override string ToString() => _stringRepresentation;
}

internal sealed class CommandBuffer
{
    private readonly Queue<string> _tokens;
    
    public bool Empty => _tokens.Count == 0;
    
    public CommandBuffer(IEnumerable<string> tokens)
    {
        _tokens = new Queue<string>(tokens);
    }
    
    public string Take()
    {
        return _tokens.TryDequeue(out var token) 
            ? token 
            : throw new InvalidOperationException("Empty buffer");
    }
}