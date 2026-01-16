using System.ComponentModel.DataAnnotations;

namespace ChatOps.Api.Integrations.Telegram;

internal sealed class TelegramConfig
{
    public const string SectionName = "Telegram";
    
    [Required]
    public required string Token { get; init; }
    
    public string AllowedChats { get; init; } = string.Empty;
    
    public long[] GetAllowedChatIds()
    {
        return AllowedChats
            .Split([',', ';'], StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Select(long.Parse)
            .ToArray();
    }
}