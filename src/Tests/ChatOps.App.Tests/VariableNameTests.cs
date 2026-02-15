using ChatOps.App.Core.Models;

namespace ChatOps.App.Tests;

public class VariableNameTests
{
    [Theory]
    [InlineData("var")]
    [InlineData("var1")]
    [InlineData("var_1")]
    [InlineData(" Var_1 ")]
    public void ShouldReturnNewInstance(string input)
    {
        _ = new VariableName(input);
    }
    
    [Fact]
    public void StartsWithDigit_ShouldThrow()
    {
        const string input = "1var";
        
        var ex = Assert.Throws<ArgumentException>(() => _ = new VariableName(input));
        
        Assert.Equal("Invalid variable name: 1var", ex.Message);
    }
    
    [Fact]
    public void ShouldFormat()
    {
        const string input = " VAR_1 ";
        
        var name = new VariableName(input);
        
        Assert.Equal("VAR_1", name.Value);
    }
    
    [Fact]
    public void ShouldConvert()
    {
        const string input = "VAR1";
        
        var name = (VariableName)input;
        
        Assert.Equal("VAR1", name.Value);
    }
}