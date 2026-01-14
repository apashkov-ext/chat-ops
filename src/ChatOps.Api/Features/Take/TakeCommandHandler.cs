using ChatOps.Api.Integrations.Telegram.Core;
using ChatOps.App.Core.Models;
using ChatOps.App.Features.Take;

namespace ChatOps.Api.Features.Take;

internal sealed class TakeCommandHandler : ITelegramCommandHandler, ICommandInfo
{
    private readonly ITakeResourceUseCase _takeResource;
    private readonly IUsersCache _usersCache;

    public TakeCommandHandler(ITakeResourceUseCase takeResource,
        IUsersCache usersCache)
    {
        _takeResource = takeResource;
        _usersCache = usersCache;
    }

    public string Command => "take <resource>";
    public string Description => "Занять указанный ресурс";

    public bool CanHandle(TelegramCommand command)
    {
        return command.Tokens.Count is not 0 && command.Tokens[0] == "take";
    }

    public async Task<TgHandlerResult> Handle(TelegramCommand command, CancellationToken ct = default)
    {
        var tokens = command.Tokens;
        if (tokens.Count != 2)
        {
            return new TelegramHandlerFailure("Invalid command syntax");
        }
        
        var holder = new HolderId(command.User.Id.ToString());
        var resourceId = new ResourceId(tokens[1]);
        
        var takeResource = await _takeResource.Execute(holder, resourceId, ct);
        return await takeResource.Match<Task<TgHandlerResult>>(
            success =>
            {
                var mention = GetMentionForHolder(holder, "Личности без имени");
                var msg = $"✅ Ресурс '{resourceId}' успешно зарезервирован для {mention}";
                var txt = new TelegramText(msg);
                var img = new TelegramImage("donotdisturb.jpg");
                return Task.FromResult<TgHandlerResult>(new TelegramReply(txt, img));
            },
            notFound =>
            {
                var txt = new TelegramText("⚠️ Ресурс не найден");
                return Task.FromResult<TgHandlerResult>(new TelegramReply(txt));
            },
            reserved =>
            {
                var mention = GetMentionForHolder(reserved.HolderId, "Личности без имени");
                var msg = $"⚠️ Ресурс уже зарезервирован для {mention}";
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