using Calculator.Core.Parsers;
using Calculator.Core.ParsingContexts;
using Calculator.Core.Tokens;

namespace Calculator.Core.Tests.Parsers;

public class SubformulaParserTest
{
    private readonly SubformulaParser parser = new();
    private readonly ParsingContext context = ParsingContext.Initial;

    [Fact]
    public void TryParse_SubformulaAtBeginning_Correct()
    {
        bool isParsed = this.parser.TryParse("(1+2)-4", this.context, out Token? token, out _);

        Assert.True(isParsed);
        this.AssertSubformulaTokenEqual(token, "1+2");
    }

    [Fact]
    public void TryParse_SubformulaNotAtBeginning_Null()
    {
        bool isParsed = this.parser.TryParse("1+(2*4)", this.context, out Token? token, out _);

        Assert.False(isParsed);
        Assert.Null(token);
    }

    [Theory]
    [InlineData("(-4)", "-4")]
    [InlineData("[0-9]", "0-9")]
    [InlineData("{7+3}", "7+3")]
    [InlineData("<1+3>*4", "1+3")]
    public void TryParse_DifferentParenthesis_Correct(string formula, string expected)
    {
        bool isParsed = this.parser.TryParse(formula, this.context, out Token? token, out _);

        Assert.True(isParsed);
        Assert.NotNull(token);
        this.AssertSubformulaTokenEqual(token, expected);
    }

    [Fact]
    public void TryParse_NoClosingParenthesis_Null()
    {
        bool isParsed = this.parser.TryParse("(1-2", this.context, out Token? token, out _);

        Assert.False(isParsed);
        Assert.Null(token);
    }

    [Fact]
    public void TryParse_ClosingParenthesisOfDifferentType_Null()
    {
        bool isParsed = this.parser.TryParse("<3}", this.context, out Token? token, out _);

        Assert.False(isParsed);
        Assert.Null(token);
    }

    [Fact]
    public void TryParse_CrossingParenthesisGroupsOfDifferentTypes_Null()
    {
        bool isParsed = this.parser.TryParse("(1+<3-4)*1>", this.context, out Token? token, out _);

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

    private void AssertSubformulaTokenEqual(Token? token, string value)
    {
        Assert.NotNull(token);
        Subformula subformulaToken = Assert.IsType<Subformula>(token);
        Assert.Equal(value, subformulaToken.Text);
    }
}