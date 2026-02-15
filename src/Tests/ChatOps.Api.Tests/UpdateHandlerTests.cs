using ChatOps.Api.Integrations.Telegram;
using ChatOps.Api.Integrations.Telegram.Core;
using ChatOps.Api.LocalAdapters.Files;
using ChatOps.Api.LocalAdapters.Users;
using Moq;
using Moq.AutoMock;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ChatOps.Api.Tests;

public class UpdateHandlerTests
{
    private readonly Update _textUpdate;
    
    private readonly UpdateHandler _handler;
    private readonly Mock<IUpdateHandlerGuard> _guard;
    private readonly Mock<ITelegramCommandHandler> _cmdHandler;
    private readonly Mock<IUpsertTelegramUser> _upsertTelegramUser;
    private readonly Mock<ITelegramChatApi> _chatApi;
    private readonly Mock<IGetStreamByFileId> _getStreamByFileId;
    private readonly ITelegramBotClient _botClient;
    
    public UpdateHandlerTests()
    {
        _textUpdate = CreateUpdate();
        
        var mocker = new AutoMocker();

        _guard = new Mock<IUpdateHandlerGuard>();
        mocker.Use(_guard);
        
        _cmdHandler = new Mock<ITelegramCommandHandler>();
        _cmdHandler.Setup(x => x.CanHandle(It.IsAny<TelegramCommand>())).Returns(true);
        _cmdHandler.Setup(x => x.Handle(
            It.IsAny<TelegramCommand>(), 
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(new TelegramReply(new TelegramText("TXT")));
        mocker.Use<IEnumerable<ITelegramCommandHandler>>([_cmdHandler.Object]);
        
        _upsertTelegramUser = new Mock<IUpsertTelegramUser>();
        mocker.Use(_upsertTelegramUser);
        
        _chatApi = new Mock<ITelegramChatApi>();
        mocker.Use(_chatApi);
        
        _getStreamByFileId = new Mock<IGetStreamByFileId>();
        mocker.Use(_getStreamByFileId);
        
        _handler = mocker.CreateInstance<UpdateHandler>();
        _botClient = new Mock<ITelegramBotClient>().Object;
    }

    [Fact]
    public async Task HandleUpdateAsync_MessageIsNull_ShouldNotInvokeHandler()
    {
        var update = new Update();
        
        await _handler.HandleUpdateAsync(_botClient, update, CancellationToken.None);
        
        _cmdHandler.Verify(
            x => x.Handle(It.IsAny<TelegramCommand>(), It.IsAny<CancellationToken>()), 
            Times.Never);
    }        
    
    [Fact]
    public async Task HandleUpdateAsync_WrongCommand_ShouldNotInvokeHandler()
    {
        _textUpdate.Message!.Text = "text";
        
        await _handler.HandleUpdateAsync(_botClient, _textUpdate, CancellationToken.None);
        
        _cmdHandler.Verify(
            x => x.Handle(It.IsAny<TelegramCommand>(), It.IsAny<CancellationToken>()), 
            Times.Never);
    }    
    
    [Fact]
    public async Task HandleUpdateAsync_GuardReturnsFalse_ShouldNotInvokeHandler()
    {
        _textUpdate.Message!.Text = "@chatops text";
        
        await _handler.HandleUpdateAsync(_botClient, _textUpdate, CancellationToken.None);
        
        _cmdHandler.Verify(
            x => x.Handle(It.IsAny<TelegramCommand>(), It.IsAny<CancellationToken>()), 
            Times.Never);    
    }    
    
    [Fact]
    public async Task HandleUpdateAsync_ShouldInvokeUpsertTelegramuser()
    {
        _textUpdate.Message!.Text = "@chatops text";
        _guard.Setup(x => x.CanHandle(It.IsAny<Update>())).Returns(true);
        
        await _handler.HandleUpdateAsync(_botClient, _textUpdate, CancellationToken.None);
        
        _upsertTelegramUser.Verify(
            x => x.Execute(It.Is<TelegramUser>(u => u.Id == 1 && u.FirstName == "John")), 
            Times.Once);    
    }

    [Fact]
    public async Task HandleUpdateAsync_ShouldInvokeHandler()
    {
        _textUpdate.Message!.Text = "@chatops text";
        _guard.Setup(x => x.CanHandle(It.IsAny<Update>())).Returns(true);

        await _handler.HandleUpdateAsync(_botClient, _textUpdate, CancellationToken.None);

        _cmdHandler.Verify(
            x => x.Handle(It.Is<TelegramCommand>(c =>
                    c.User == new TelegramUser(1, "John", null, null)
                    && c.Tokens.Contains("text")),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }    
    
    [Fact]
    public async Task HandleUpdateAsync_NoAnyHandlers_ShouldSendMessage()
    {
        _textUpdate.Message!.Text = "@chatops text";
        _guard.Setup(x => x.CanHandle(It.IsAny<Update>())).Returns(true);
        _cmdHandler.Setup(x => x.CanHandle(It.IsAny<TelegramCommand>())).Returns(false);

        await _handler.HandleUpdateAsync(_botClient, _textUpdate, CancellationToken.None);

        _chatApi.Verify(
            x => x.SendHtmlMessage(
                It.Is<long>(c => c == 1), 
                "⚠️ Неизвестная команда",
                It.IsAny<CancellationToken>()),
            Times.Once);
    }    
    
    [Fact]
    public async Task HandleUpdateAsync_NoAnyhandlers_ShouldNotInvokeHandler()
    {
        _textUpdate.Message!.Text = "@chatops text";
        _guard.Setup(x => x.CanHandle(It.IsAny<Update>())).Returns(true);
        _cmdHandler.Setup(x => x.CanHandle(It.IsAny<TelegramCommand>())).Returns(false);

        await _handler.HandleUpdateAsync(_botClient, _textUpdate, CancellationToken.None);

        _cmdHandler.Verify(x => x.Handle(
                It.IsAny<TelegramCommand>(), 
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task HandleUpdateAsync_HandlerReturnsFailure_ShouldSendMessage()
    {
        _textUpdate.Message!.Text = "@chatops text";
        _guard.Setup(x => x.CanHandle(It.IsAny<Update>())).Returns(true);
        _cmdHandler.Setup(x => x.Handle(
                It.IsAny<TelegramCommand>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new TelegramHandlerFailure("My Error"));

        await _handler.HandleUpdateAsync(_botClient, _textUpdate, CancellationToken.None);

        _chatApi.Verify(
            x => x.SendHtmlMessage(
                It.Is<long>(c => c == 1), 
                "⛔ My Error",
                It.IsAny<CancellationToken>()),
            Times.Once);
    }    
    
    [Fact]
    public async Task HandleUpdateAsync_HandlerReturnsUnknownCommand_ShouldSendMessage()
    {
        _textUpdate.Message!.Text = "@chatops text";
        _guard.Setup(x => x.CanHandle(It.IsAny<Update>())).Returns(true);
        _cmdHandler.Setup(x => x.Handle(
                It.IsAny<TelegramCommand>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new UnknownCommand());

        await _handler.HandleUpdateAsync(_botClient, _textUpdate, CancellationToken.None);

        _chatApi.Verify(
            x => x.SendHtmlMessage(
                It.Is<long>(c => c == 1), 
                "⚠️ Неизвестная команда",
                It.IsAny<CancellationToken>()),
            Times.Once);
    }    
    
    [Fact]
    public async Task HandleUpdateAsync_HandlerReturnsText_ShouldSendTextMessage()
    {
        _textUpdate.Message!.Text = "@chatops text";
        _guard.Setup(x => x.CanHandle(It.IsAny<Update>())).Returns(true);
        _cmdHandler.Setup(x => x.Handle(
                It.IsAny<TelegramCommand>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new TelegramReply(new TelegramText("TXT")));

        await _handler.HandleUpdateAsync(_botClient, _textUpdate, CancellationToken.None);

        _chatApi.Verify(
            x => x.SendHtmlMessage(
                It.Is<long>(c => c == 1), 
                "TXT",
                It.IsAny<CancellationToken>()),
            Times.Once);
    }    
    
    [Fact]
    public async Task HandleUpdateAsync_HandlerReturnsImage_ShouldSendImage()
    {
        _textUpdate.Message!.Text = "@chatops text";
        _guard.Setup(x => x.CanHandle(It.IsAny<Update>())).Returns(true);
        _cmdHandler.Setup(x => x.Handle(
                It.IsAny<TelegramCommand>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new TelegramReply(new TelegramImage("image.jpg")));
        _getStreamByFileId.Setup(x => x.Execute(It.IsAny<string>()))
            .Returns(new MemoryStream([]));

        await _handler.HandleUpdateAsync(_botClient, _textUpdate, CancellationToken.None);

        _chatApi.Verify(
            x => x.SendImage(
                It.Is<long>(c => c == 1), 
                It.IsAny<Stream>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }    
    
    [Fact]
    public async Task HandleUpdateAsync_HandlerReturnsComplexReply_ShouldSendBothMessages()
    {
        _textUpdate.Message!.Text = "@chatops text";
        _guard.Setup(x => x.CanHandle(It.IsAny<Update>())).Returns(true);
        var reply = new TelegramReply(new TelegramText("TXT"), new TelegramImage("image.jpg"));
        _cmdHandler.Setup(x => x.Handle(
                It.IsAny<TelegramCommand>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(reply);
        _getStreamByFileId.Setup(x => x.Execute(It.IsAny<string>()))
            .Returns(new MemoryStream([]));

        await _handler.HandleUpdateAsync(_botClient, _textUpdate, CancellationToken.None);

        _chatApi.Verify(
            x => x.SendHtmlMessage(
                It.Is<long>(c => c == 1), 
                "TXT",
                It.IsAny<CancellationToken>()),
            Times.Once);        
        
        _chatApi.Verify(
            x => x.SendImage(
                It.Is<long>(c => c == 1), 
                It.IsAny<Stream>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    private static Update CreateUpdate()
    {
        return new Update
        {
            Message = new Message
            {
                From = new User
                {
                    Id = 1, 
                    FirstName = "John"
                },
                Text = "text",
                Chat =  new Chat
                {
                    Id = 1,
                    Type = ChatType.Group
                }
            }
        };
    }
}