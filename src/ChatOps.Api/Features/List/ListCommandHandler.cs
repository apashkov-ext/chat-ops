using System.Text;
using ChatOps.Api.Integrations.Telegram;
using ChatOps.Api.Integrations.Telegram.Core;
using ChatOps.App.Core.Models;
using ChatOps.App.Features.List;

namespace ChatOps.Api.Features.List;

internal sealed class ListCommandHandler : ITelegramCommandHandler, ICommandInfo
{
    private readonly IListResourcesUseCase _listResourcesUseCase;
    private readonly IUsersCache _usersStore;

    public ListCommandHandler(IListResourcesUseCase listResourcesUseCase, IUsersCache usersStore)
    {
        _listResourcesUseCase = listResourcesUseCase;
        _usersStore = usersStore;
    }

    public string Command => "list";
    public string Description => "Вывести список ресурсов";

    public bool CanHandle(TelegramCommand collection)
    {
        return collection.Tokens is ["list"];
    }

    public async Task<TgHandlerResult> Handle(TelegramCommand tokens, CancellationToken ct = default)
    {
        var resources = await _listResourcesUseCase.Execute(ct);
        
        var pairs = GetPairs(resources).ToArray();
        var list = BuildList(pairs);
        
        var text = new TelegramText(list);
        return new TelegramReply(text);
    }

    private IEnumerable<ResourceHolderPair> GetPairs(IReadOnlyList<Resource> resources)
    {
        foreach (var resource in resources)
        {
            TelegramUser? holder = null;
            if (resource.Holder != null)
            {
                var id = long.Parse(resource.Holder.Value);
                holder = _usersStore.Find(id);
            }
            
            yield return new ResourceHolderPair(resource, holder);
        }
    }
    
    private static string BuildList(ResourceHolderPair[] pairs)
    {
        var resources = pairs.Length == 0 
            ? ["[пусто]"] 
            : pairs.Select(Stringify).Select((el, idx) => $" {idx + 1}. {el}");
        
        return
            $"""
             {TgHtml.B("Список ресурсов")}

             {string.Join(Environment.NewLine, resources)}
             """
            ;
    }
    
    private static string Stringify(ResourceHolderPair pair)
    {
        var sb = new StringBuilder(pair.Resource.Id.Value);
        sb.Append(", ");

        switch (pair.Resource.State)
        {
            case ResourceState.Free:
                sb.Append("свободен");
                break;
            
            case ResourceState.Reserved:
                sb.Append("занят ");

                sb.Append(pair.Holder?.GetMention() ?? "неизвестно кем");
                break;
            
            default:
                sb.Append("неведомый статус, не замэпилось");
                break;
        }

        return sb.ToString();
    }
    
    private record ResourceHolderPair(Resource Resource, TelegramUser? Holder);
}