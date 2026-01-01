using ChatOps.Api.Features.TelegramMessageHandler.Handling;
using ChatOps.Api.Integrations.Telegram.Core;
using ChatOps.App.UseCases.ListResources;

namespace ChatOps.Api.Features.Env.List;

internal sealed class ListCommandHandler : ITelegramCommandHandler
{
    private readonly IListResourcesUseCase _listResourcesUseCase;

    public ListCommandHandler(IListResourcesUseCase listResourcesUseCase)
    {
        _listResourcesUseCase = listResourcesUseCase;
    }
    
    public bool CanHandle(CommandTokenCollection tokens)
    {
        throw new NotImplementedException();
    }

    public async Task<TgHandlerResult> Handle(CommandTokenCollection tokens, CancellationToken ct = default)
    {
        var response = await _listResourcesUseCase.Execute(ct);
        var list = Stringifier.BuildList(response);
        return new TelegramReply(list);
    }
}