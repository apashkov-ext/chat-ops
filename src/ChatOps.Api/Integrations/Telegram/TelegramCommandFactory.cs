using ChatOps.Api.Integrations.Telegram.Core;

namespace ChatOps.Api.Integrations.Telegram;

internal static class TelegramCommandFactory
{
    public static bool TryCreate(Message message, out TelegramCommand command)
    {
        if (message.From is null)
        {
            command = TelegramCommand.Empty(TelegramUser.Unknown);
            return false;
        }
        
        var text = message.Text?.Trim() ?? string.Empty;
        var from = message.From;
        var user = new TelegramUser(from.Id, from.FirstName, from.LastName, from.Username);
        
        if (message.Chat.Type != ChatType.Private && !text.StartsWith($"{Constants.CommandPrefix}"))
        {
            command = TelegramCommand.Empty(user);
            return false;
        }
        
        // личная переписка с ботом, можно без официоза
        text = Clean(text);
        command = TelegramCommand.Parse(user, text);
        return true;
    }    
    
    private static string Clean(string text)
    {
        return text.Replace(Constants.CommandPrefix, string.Empty).Trim();
    }
}