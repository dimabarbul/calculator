using Calculator.Core.Parsers;
using Calculator.Core.ParsingContexts;
using Calculator.Core.Tokens;

namespace Calculator.Core.Tests.Parsers;

public class VariableParserTest
{
    private readonly VariableParser parser = new();
    private readonly ParsingContext context = ParsingContext.Initial;

    [Fact]
    public void TryParse_VariableAtBeginning_Correct()
    {
        bool isParsed = this.parser.TryParse("$test", this.context, out Token? token, out _);

        Assert.True(isParsed);
        this.AssertVariableTokenEqual(token, "test");
    }

    [Fact]
    public void TryParse_VariableNotAtBeginning_Null()
    {
        bool isParsed = this.parser.TryParse("2*$test", this.context, out Token? token, out _);

        Assert.False(isParsed);
        Assert.Null(token);
    }

    [Fact]
    public void TryParse_VariableFollowedByParenthesis_Correct()
    {
        bool isParsed = this.parser.TryParse("$test()", this.context, out Token? token, out _);

        Assert.True(isParsed);
        this.AssertVariableTokenEqual(token, "test");
    }

    [Fact]
    public void TryParse_StartsWithDigit_Null()
    {
        bool isParsed = this.parser.TryParse("$4fun", this.context, out Token? token, out _);

        Assert.False(isParsed);
        Assert.Null(token);
    }

    [Fact]
    public void TryParse_EmptyString_Null()
    {
        bool isParsed = this.parser.TryParse(string.Empty, this.context, out Token? token, out _);

        Assert.False(isParsed);
        Assert.Null(token);
    }

    private void AssertVariableTokenEqual(Token? token, string value)
    {
        Assert.NotNull(token);
        Variable variableToken = Assert.IsType<Variable>(token);
        Assert.Equal(value, variableToken.Name);
    }
}