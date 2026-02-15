using ChatOps.App.Core;

namespace ChatOps.Api.Integrations.Telegram.Core;

internal sealed class TelegramCommand : ValueObject
{
    private const char _separator = ' ';
    private readonly string _stringRepresentation;

    public TelegramUser User { get; }
    public IReadOnlyList<string> Tokens { get; }

    internal TelegramCommand(TelegramUser user, IEnumerable<string> tokens)
    {
        User = user;
        Tokens = [..tokens];
        _stringRepresentation = string.Join(_separator, [$"@{user}: ", ..Tokens]);
    }
    
    public static TelegramCommand Parse(TelegramUser user, string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return new TelegramCommand(user, []);
        }

        var tokens = input.Split(_separator, 
            StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        return new TelegramCommand(user, tokens);
    }

    public static TelegramCommand Empty(TelegramUser user) => new(user, []);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return _stringRepresentation;
    }

    protected override string GetStringRepresentation() => _stringRepresentation;
}