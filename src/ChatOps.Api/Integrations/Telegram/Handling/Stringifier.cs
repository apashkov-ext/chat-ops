using System.Text;
using ChatOps.App.Core.Models;
using ChatOps.App.UseCases.ListResources;

namespace ChatOps.Api.Integrations.Telegram.Handling;

internal static class Stringifier
{
    public static string BuildHelpText()
    {
        return
            $"""
             {TgHtml.B("Доступные команды")}

              {TgHtml.Code(WellKnownCommandTokens.Env, WellKnownCommandTokens.List)}
              {TgHtml.Code(WellKnownCommandTokens.Env, WellKnownCommandTokens.Take, "dev1", "[branch]")}
              {TgHtml.Code(WellKnownCommandTokens.Env, WellKnownCommandTokens.Release, "dev1")}
             """
            ;
    }

    public static string BuildList(IReadOnlyList<Resource> model)
    {
        var resources = model.Count == 0 
            ? ["[пусто]"] 
            : model.Select(Stringify).Select((el, idx) => $" {idx + 1}. {el}");
        
        return
            $"""
             {TgHtml.B("Список ресурсов")}

             {string.Join(Environment.NewLine, resources)}
             """
            ;
    }

    private static string Stringify(Resource resource)
    {
        var sb = new StringBuilder(resource.Name);
        sb.Append(", ");

        switch (resource.State)
        {
            case ResourceState.Free:
                sb.Append("свободен");
                break;
            
            case ResourceState.Reserved:
                sb.Append("занят ");

                sb.Append(resource.Holder ?? "неизвестно кем");
                break;
            
            default:
                sb.Append("неведомый статус, не замэпилось");
                break;
        }

        return sb.ToString();
    }
}

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