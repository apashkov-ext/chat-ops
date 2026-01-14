using ChatOps.Api.Integrations.Telegram.Core;
using ChatOps.Api.Storage.Users;
using ChatOps.App.Core.Models;
using ChatOps.App.Features.Free;

namespace ChatOps.Api.Features.Free;

internal sealed class FreeCommandHandler : ITelegramCommandHandler, ICommandInfo
{
    private readonly IFreeResourceUseCase _freeResourceUseCase;
    private readonly IUsersCache _usersCache;
    public string Command => "free <resource>";
    public string Description => "Освободить указанный ресурс";

    public FreeCommandHandler(IFreeResourceUseCase freeResourceUseCase,
        IUsersCache usersCache)
    {
        _freeResourceUseCase = freeResourceUseCase;
        _usersCache = usersCache;
    }

    public bool CanHandle(TelegramCommand command)
    {
        return command.Tokens.Count is not 0 && command.Tokens[0] == "free";
    }

    public async Task<TgHandlerResult> Handle(TelegramCommand command, CancellationToken ct = default)
    {
        var tokens = command.Tokens;
        if (tokens.Count != 2)
        {
            return new TelegramHandlerFailure("Invalid command syntax");
        }
        
        var holderId = new HolderId(command.User.Id.ToString());
        var resourceId = new ResourceId(tokens[1]);
        
        var freeResource = await _freeResourceUseCase.Execute(holderId, resourceId, ct);
        return await freeResource.Match<Task<TgHandlerResult>>(
            success =>
            {
                var msg = $"✅ Ресурс '{resourceId}' освобожден";
                var txt = new TelegramText(msg);
                var img = new TelegramImage("myjobisdone.jpg");
                return Task.FromResult<TgHandlerResult>(new TelegramReply(txt, img));
            }, 
            notFound =>
            {
                var txt = new TelegramText("⚠️ Ресурс не найден");
                return Task.FromResult<TgHandlerResult>(new TelegramReply(txt));
            }, 
            inUse =>
            {
                var mention = GetMentionForHolder(inUse.HolderId, "Личности без имени");
                var msg = $"⚠️ Ресурс зарезервирован для {mention}";
                var txt = new TelegramText(msg);
                return Task.FromResult<TgHandlerResult>(new TelegramReply(txt));
            }, 
            failure => Task.FromResult<TgHandlerResult>(new TelegramHandlerFailure(failure.Error))
        );
    }
    
    private string GetMentionForHolder(HolderId holder, string nonameUserPlaceholder)
    {
        var userId = long.Parse(holder.Value);
        var user = _usersCache.Find(userId);
                
        var mention = user?.GetMention() ?? TelegramUser.GetMention(userId, nonameUserPlaceholder);
        return mention;
    }
}