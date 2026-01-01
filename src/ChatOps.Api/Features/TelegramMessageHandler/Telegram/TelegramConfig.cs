using System.ComponentModel.DataAnnotations;

namespace ChatOps.Api.Features.TelegramMessageHandler.Telegram;

internal sealed class TelegramConfig
{
    public const string SectionName = "Telegram";
    
    [Required]
    public required string Token { get; init; }
}