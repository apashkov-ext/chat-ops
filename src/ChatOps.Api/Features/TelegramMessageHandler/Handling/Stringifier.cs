using System.Text;
using ChatOps.App.Core.Models;

namespace ChatOps.Api.Features.TelegramMessageHandler.Handling;

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