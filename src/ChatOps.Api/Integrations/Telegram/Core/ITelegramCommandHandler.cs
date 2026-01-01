using ChatOps.Api.Features.TelegramMessageHandler.Handling;

namespace ChatOps.Api.Integrations.Telegram.Core;

internal interface ITelegramCommandHandler
{
    bool CanHandle(CommandTokenCollection tokens);
    Task<TgHandlerResult> Handle(CommandTokenCollection tokens, CancellationToken ct = default);
}