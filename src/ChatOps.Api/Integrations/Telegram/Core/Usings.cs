global using Telegram.Bot;
global using Telegram.Bot.Types;
global using Telegram.Bot.Types.Enums;

global using TgHandlerResult = OneOf.OneOf<
    ChatOps.Api.Integrations.Telegram.Core.TelegramReply, 
    ChatOps.Api.Integrations.Telegram.Core.TelegramHandlerFailure, 
    ChatOps.Api.Integrations.Telegram.Core.UnknownCommand
>;
