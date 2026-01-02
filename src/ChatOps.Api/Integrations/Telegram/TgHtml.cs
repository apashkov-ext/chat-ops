namespace ChatOps.Api.Integrations.Telegram;

internal static class TgHtml
{
    public static string B(params string[] s) => $"<b>{Esc(Join(s))}</b>";
    public static string I(string s) => $"<i>{Esc(s)}</i>";
    public static string Code(params string[] s) => $"<code>{Esc(Join(s))}</code>";
    public static string Pre(params string[] s) => $"<pre>{Esc(Join(s))}</pre>";
    public static string U(params string[] s) => $"<u>{Esc(Join(s))}</u>";
    public static string S(params string[] s) => $"<s>{Esc(Join(s))}</s>";

    public static string Link(string text, string url) =>
        $"<a href=\"{Esc(url)}\">{Esc(text)}</a>";

    public static string Esc(string s) => System.Net.WebUtility.HtmlEncode(s);
    
    private static string Join(params string[] s) => string.Join(" ", s);
}