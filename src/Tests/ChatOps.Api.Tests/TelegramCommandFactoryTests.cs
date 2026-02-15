using ChatOps.Api.Integrations.Telegram;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ChatOps.Api.Tests;

public class TelegramCommandFactoryTests
{
    private readonly Message _message = new ()
    {
        From = new User
        {
            Id = 888,
            FirstName = "John"
        },
        Chat = new Chat(),
        Text = string.Empty
    };

    
    [Theory]
    [InlineData(ChatType.Channel)]
    [InlineData(ChatType.Group)]
    [InlineData(ChatType.Supergroup)]
    [InlineData(ChatType.Sender)]
    public void TryCreate_PublicChat_ShouldReturnFalseIfDoesntStartWithPrefix(ChatType chatType)
    {
        _message.Chat.Type = chatType;
        _message.Text = "list";

        var result = TelegramCommandFactory.TryCreate(_message, out _);
        
        Assert.False(result);
    }    
    
    [Theory]
    [InlineData(ChatType.Channel)]
    [InlineData(ChatType.Group)]
    [InlineData(ChatType.Supergroup)]
    [InlineData(ChatType.Sender)]
    public void TryCreate_PublicChat_ShouldReturnTrueIfStartsWithPrefix(ChatType chatType)
    {
        _message.Chat.Type = chatType;
        _message.Text = "@chatops list";

        var result = TelegramCommandFactory.TryCreate(_message, out _);
        
        Assert.True(result);
    }    
    
    [Fact]
    public void TryCreate_PrivateChat_ShouldRemovePrefix()
    {
        _message.Chat.Type = ChatType.Private;
        _message.Text = "@chatops list";

        TelegramCommandFactory.TryCreate(_message, out var command);
        
        Assert.Single(command.Tokens, t => t == "list");
    }    
}