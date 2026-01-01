using ChatOps.Api.Features.TelegramMessageHandler.Handling;
using ChatOps.App.Core.Models;

namespace ChatOps.Api.Tests;

public class StringifierTests
{
    [Fact]
    public void BuildHelpText_SheuldReturnHelpMessage()
    {
        const string expectedMessage = """
                                       <b>Доступные команды</b>
                                       
                                        <code>env list</code>
                                        <code>env take dev1 [branch]</code>
                                        <code>env release dev1</code>
                                       """;

        var actualMessage = Stringifier.BuildHelpText();
        
        Assert.Equal(expectedMessage, actualMessage);
    }

    [Fact]
    public void BuildList_ShouldReturnEmptyList()
    {
        var expectedMessage =
                $"""
                 <b>Список ресурсов</b>
                 
                 [пусто]
                 """
            ;
        
        Resource[] model = [];
        var actualMessage = Stringifier.BuildList(model);
        
        Assert.Equal(expectedMessage, actualMessage);
    }    
    
    [Fact]
    public void BuildList_ShouldReturnNonEmptyList()
    {
        var expectedMessage =
                """
                <b>Список ресурсов</b>
                
                 1. dev, свободен
                 2. dev1, занят @user
                 3. dev2, свободен
                """
            ;
        
        Resource[] model = 
        [
            new Resource
            {
                Id = new ResourceId(Guid.NewGuid().ToString()),
                Name = "dev",
                State = ResourceState.Free
            },
            new Resource
            {
                Id = new ResourceId(Guid.NewGuid().ToString()),
                Name = "dev1",
                State = ResourceState.Reserved,
                Holder = "@user"
            },
            new Resource
            {
                Id = new ResourceId(Guid.NewGuid().ToString()),
                Name = "dev2",
                State = ResourceState.Free
            }
        ];
        var actualMessage = Stringifier.BuildList(model);
        
        Assert.Equal(expectedMessage, actualMessage);
    }
}