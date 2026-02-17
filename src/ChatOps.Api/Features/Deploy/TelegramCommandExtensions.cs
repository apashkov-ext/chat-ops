using ChatOps.Api.Integrations.Telegram.Core;
using ChatOps.App.Core.Models;

namespace ChatOps.Api.Features.Deploy;

internal static class TelegramCommandExtensions
{
    public static IEnumerable<Variable> GetVariables(this TelegramCommand command)
    {
        foreach (var arg in command.Tokens)
        {
            var parts = arg.Split('=', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (parts.Length != 2)
            {
                continue;
            }
            
            yield return new Variable(parts[0], parts[1]);
        }
    }
}