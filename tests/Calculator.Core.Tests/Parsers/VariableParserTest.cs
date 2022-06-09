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
        Token? token;

        this.parser.TryParse("$test", this.context, out token, out _);
        this.AssertVariableTokenEqual(token, "test");
    }

    [Fact]
    public void TryParse_VariableNotAtBeginning_Null()
    {
        Token? token;

        this.parser.TryParse("2*$test", this.context, out token, out _);
        Assert.Null(token);
    }

    [Fact]
    public void TryParse_VariableFollowedByParenthesis_Correct()
    {
        Token? token;

        this.parser.TryParse("$test()", this.context, out token, out _);
        this.AssertVariableTokenEqual(token, "test");
    }

    [Fact]
    public void TryParse_StartsWithDigit_Null()
    {
        Token? token;

        this.parser.TryParse("$4fun", this.context, out token, out _);
        Assert.Null(token);
    }

    [Fact]
    public void TryParse_EmptyString_Null()
    {
        Token? token;

        this.parser.TryParse(string.Empty, this.context, out token, out _);
        Assert.Null(token);
    }

    private void AssertVariableTokenEqual(Token? token, string value)
    {
        Assert.NotNull(token);
        Variable variableToken = Assert.IsType<Variable>(token);
        Assert.Equal(value, variableToken.Name);
    }
}