using ChatOps.Api.Integrations.Telegram.Handling;
using ChatOps.App.Core.Models;
using ChatOps.App.UseCases.ListResources;

namespace ChatOps.Api.Tests;

public class StringifierTests
{
    [Fact]
    public void BuildHelpText_SheuldReturnHelpMessage()
    {
        const string expectedMessage = """
                                       Доступные команды:
                                       env list
                                       env take dev1 [my-branch]
                                       env release dev1
                                       """;

        var actualMessage = Stringifier.BuildHelpText();
        
        Assert.Equal(expectedMessage, actualMessage);
    }

    [Fact]
    public void BuildList_ShouldReturnEmptyList()
    {
        var expectedMessage =
                $"""
                 Список ресурсов:
                 [пусто]
                 """
            ;
        
        ResourceInfo[] model = [];
        var actualMessage = Stringifier.BuildList(model);
        
        Assert.Equal(expectedMessage, actualMessage);
    }    
    
    [Fact]
    public void BuildList_ShouldReturnNonEmptyList()
    {
        var expectedMessage =
                """
                Список ресурсов:
                1. dev, свободен
                2. dev1, занят @user
                3. dev2, свободен
                """
            ;
        
        ResourceInfo[] model = 
        [
            new (new Resource
            {
                Id = new ResourceId(Guid.NewGuid().ToString()),
                Name = "dev",
                State = ResourceState.Free
            }, null),
            new (new Resource
            {
                Id = new ResourceId(Guid.NewGuid().ToString()),
                Name = "dev1",
                State = ResourceState.Reserved
            }, new Holder
            {
                Id = new HolderId(Guid.NewGuid().ToString()),
                Name = "@user"
            }),
            new (new Resource
            {
                Id = new ResourceId(Guid.NewGuid().ToString()),
                Name = "dev2",
                State = ResourceState.Free
            }, null)
        ];
        var actualMessage = Stringifier.BuildList(model);
        
        Assert.Equal(expectedMessage, actualMessage);
    }
}