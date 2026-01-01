global using Telegram.Bot;
global using Telegram.Bot.Types;
global using Telegram.Bot.Types.Enums;

global using TgHandlerResult = OneOf.OneOf<
    ChatOps.Api.Features.TelegramMessageHandler.Handling.TelegramReply, 
    ChatOps.Api.Features.TelegramMessageHandler.Handling.TelegramHandlerFailure, 
    ChatOps.Api.Features.TelegramMessageHandler.Handling.UnknownCommand
>;
