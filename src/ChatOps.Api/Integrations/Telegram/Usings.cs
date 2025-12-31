global using Telegram.Bot;
global using Telegram.Bot.Types;
global using Telegram.Bot.Types.Enums;

global using TgHandlerResult = OneOf.OneOf<
    ChatOps.Api.Integrations.Telegram.Handling.TelegramReply, 
    ChatOps.Api.Integrations.Telegram.Handling.TelegramHandlerFailure, 
    ChatOps.Api.Integrations.Telegram.Handling.UnknownCommand
>;
