using ChatOps.Api.Integrations.Telegram;
using ChatOps.Api.Integrations.Telegram.Core;
using ChatOps.App.Core.Models;
using ChatOps.App.Features.Deploy;

namespace ChatOps.Api.Features.Deploy;

internal sealed class DeployCommandHandler : ITelegramCommandHandler, ICommandInfo
{
    private readonly IDeployUseCase _deployUseCase;
    public string Command => "deploy <resource> <branch> [ENV=VAL]";
    public string Description => "Установить ветку на ресурс";

    public DeployCommandHandler(IDeployUseCase deployUseCase)
    {
        _deployUseCase = deployUseCase;
    }
    
    public bool CanHandle(TelegramCommand command)
    {
        return command.Tokens.Count is not 0 && command.Tokens[0] == "deploy";
    }

    public async Task<TgHandlerResult> Handle(TelegramCommand command, CancellationToken ct = default)
    {
        var tokens = command.Tokens;
        if (tokens.Count < 3)
        {
            return new TelegramHandlerFailure("Invalid command syntax");
        }
        
        var holder = new HolderId(command.User.Id.ToString());
        var resourceId = new ResourceId(tokens[1]);
        var @ref = new Ref(tokens[2]);
        var variables = command.GetVariables();
        
        var deploy = await _deployUseCase.Execute(holder, resourceId, @ref, variables, ct);
        return await deploy.Match<Task<TgHandlerResult>>(
            success =>
            {
                var formattedId = $"#{success.Pipeline.Id}";
                var link = TgHtml.Link(formattedId, success.Pipeline.Link.Value);
                var msg = $"""
                           ✅ Пайплайн {link} запущен.  
                           ресурс: {resourceId}
                           ветка/тег: {@ref}
                           """;
                var txt = new TelegramText(msg);
                return Task.FromResult<TgHandlerResult>(new TelegramReply(txt));
            },
            _ =>
            {
                var txt = new TelegramText("⚠️ Ресурс не найден");
                return Task.FromResult<TgHandlerResult>(new TelegramReply(txt));            
            },
            _ =>
            {
                var txt = new TelegramText("⚠️ Сначала нужно зарезервивовать этот ресурс");
                return Task.FromResult<TgHandlerResult>(new TelegramReply(txt));            
            },
            _ =>
            {
                var txt = new TelegramText("⚠️ Ветка/тег не найдена");
                return Task.FromResult<TgHandlerResult>(new TelegramReply(txt));
            },            
            inProcess =>
            {
                var formattedId = $"#{inProcess.Pipeline.Id}";
                var link = TgHtml.Link(formattedId, inProcess.Pipeline.Link.Value);
                var msg = $"""
                           ℹ️ Пайплайн {link} уже запущен и выполняется.  
                           ресурс: {resourceId}
                           ветка/тег: {@ref}
                           """;
                var txt = new TelegramText(msg);
                return Task.FromResult<TgHandlerResult>(new TelegramReply(txt));
            },
            _ =>
            {
                const string msg = """
                                   Не получилось запустить пайплайн.  
                                   Пойдите и выясните причину.
                                   """;
                return Task.FromResult<TgHandlerResult>(new TelegramHandlerFailure(msg));
            });
    }
}