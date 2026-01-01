using Telegram.Bot.Polling;

namespace ChatOps.Api.Features.TelegramMessageHandler;

internal sealed class TelegramPoller : BackgroundService
{
    private readonly ITelegramBotClient _bot;
    private readonly IUpdateHandler _updateHandler;
    private readonly ILogger<TelegramPoller> _logger;

    public TelegramPoller(ITelegramBotClient bot,
        IUpdateHandler updateHandler,
        ILogger<TelegramPoller> logger)
    {
        _bot = bot;
        _updateHandler = updateHandler;
        _logger = logger;
    }
    
    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        if (ct.IsCancellationRequested)
        {
            return;
        }
        
        try
        {
            await InitializeBot(ct);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to initialize Telegram Bot");
        }
    }

    private async Task InitializeBot(CancellationToken ct)
    {
        var user = await _bot.GetMe(ct);
        _logger.LogInformation("Starting Telegram Bot as @{username:l} ({Id})", 
            user.Username,
            user.Id);

        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates =
            [
                UpdateType.Message
            ]
        };
        
        _bot.StartReceiving(updateHandler: _updateHandler, 
            receiverOptions: receiverOptions, 
            cancellationToken: ct);
        
        await Task.Delay(Timeout.Infinite, ct);
    }
}