namespace ChatOps.Api.Features.TelegramMessageHandler.Handling;

internal static class WellKnownCommandTokens
{
    public const string Start = "/start";
    public const string Help = "/help";
    public const string Env = "env";
    public const string List = "env list";
    public const string Take = "env take {res}";
    public const string Release = "env release {res}";
}