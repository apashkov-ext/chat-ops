using ChatOps.Api.Integrations.Telegram;
using ChatOps.Api.Integrations.Telegram.Core;

namespace ChatOps.Api.Features.Help;

internal sealed class HelpCommandHandler : ITelegramCommandHandler
{
    public bool CanHandle(CommandTokenCollection collection)
    {
        return collection.Tokens is ["/help"];
    }

    public async Task<TgHandlerResult> Handle(CommandTokenCollection collection, CancellationToken ct = default)
    {
        var help = $"""
                    {TgHtml.B("Доступные команды")}

                     {TgHtml.Code("list")}
                     {TgHtml.Code("take", "<env>", "[branch]")}
                     {TgHtml.Code("release", "<env>")}
                    """;
        return new TelegramReply(help);
    }
}