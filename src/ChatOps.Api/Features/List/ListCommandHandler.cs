using System.Text;
using ChatOps.Api.Integrations.Telegram;
using ChatOps.Api.Integrations.Telegram.Core;
using ChatOps.App.Core.Models;
using ChatOps.App.UseCases.ListResources;

namespace ChatOps.Api.Features.List;

internal sealed class ListCommandHandler : ITelegramCommandHandler
{
    private readonly IListResourcesUseCase _listResourcesUseCase;

    public ListCommandHandler(IListResourcesUseCase listResourcesUseCase)
    {
        _listResourcesUseCase = listResourcesUseCase;
    }
    
    public bool CanHandle(CommandTokenCollection collection)
    {
        return collection.Tokens is ["list"];
    }

    public async Task<TgHandlerResult> Handle(CommandTokenCollection tokens, CancellationToken ct = default)
    {
        var response = await _listResourcesUseCase.Execute(ct);
        var list = BuildList(response);
        return new TelegramReply(list);
    }
    
    private static string BuildList(IReadOnlyList<Resource> model)
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