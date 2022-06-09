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
        Token? token;
        this.parser.TryParse("(1+2)-4", this.context, out token, out _);

        this.AssertSubformulaTokenEqual(token, "1+2");
    }

    [Fact]
    public void TryParse_SubformulaNotAtBeginning_Null()
    {
        Token? token;
        this.parser.TryParse("1+(2*4)", this.context, out token, out _);

        Assert.Null(token);
    }

    [Fact]
    public void TryParse_DifferentParenthesis_Correct()
    {
        Token? token;

        this.parser.TryParse("[0-9]", this.context, out token, out _);
        Assert.NotNull(token);
        this.AssertSubformulaTokenEqual(token, "0-9");

        this.parser.TryParse("{7+3}", this.context, out token, out _);
        Assert.NotNull(token);
        this.AssertSubformulaTokenEqual(token, "7+3");

        this.parser.TryParse("<1+3>*4", this.context, out token, out _);
        Assert.NotNull(token);
        this.AssertSubformulaTokenEqual(token, "1+3");
    }

    [Fact]
    public void TryParse_NoClosingParenthesis_Null()
    {
        Token? token;

        this.parser.TryParse("(1-2", this.context, out token, out _);
        Assert.Null(token);
    }

    [Fact]
    public void TryParse_ClosingParenthesisOfDifferentType_Null()
    {
        Token? token;

        this.parser.TryParse("<3}", this.context, out token, out _);
        Assert.Null(token);
    }

    [Fact]
    public void TryParse_CrossingParenthesisGroupsOfDifferentTypes_Null()
    {
        Token? token;

        this.parser.TryParse("(1+<3-4)*1>", this.context, out token, out _);
        Assert.Null(token);
    }

    [Fact]
    public void TryParse_EmptyString_Null()
    {
        Token? token;

        this.parser.TryParse(string.Empty, this.context, out token, out _);
        Assert.Null(token);
    }

    private void AssertSubformulaTokenEqual(Token? token, string value)
    {
        Assert.NotNull(token);
        Subformula subformulaToken = Assert.IsType<Subformula>(token);
        Assert.Equal(value, subformulaToken.Text);
    }
}