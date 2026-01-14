using ChatOps.Api.Integrations.Telegram;
using ChatOps.Api.Integrations.Telegram.Core;

namespace ChatOps.Api.Features.Help;

internal sealed class HelpCommandHandler : ITelegramCommandHandler, ICommandInfo
{
    private readonly ICommandInfo[] _infos;
    public string Command => "help";
    public string Description => "Показать справку";

    public HelpCommandHandler(IEnumerable<ICommandInfo> infos)
    {
        _infos = infos.Append(this).ToArray();
    }

    public bool CanHandle(TelegramCommand collection)
    {
        return collection.Tokens is ["help"];
    }

    public async Task<TgHandlerResult> Handle(TelegramCommand collection, CancellationToken ct = default)
    {
        var lines = _infos.Select(BuildInfo);
        var separator = Environment.NewLine + Environment.NewLine;
        var help = $"""
                    ℹ️ {TgHtml.B("Доступные команды:")}
                    
                    {string.Join(separator, lines)}
                    """;
        var text = new TelegramText(help);
        return new TelegramReply(text);
    }

    private static string BuildInfo(ICommandInfo info)
    {
        var text = $"""
                     {TgHtml.B(info.Command)}
                     {TgHtml.Esc(info.Description)}
                    """;
        return text;
    }
}