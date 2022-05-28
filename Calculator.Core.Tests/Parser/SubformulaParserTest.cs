using Calculator.Core.Enum;
using Calculator.Core.Parser;
using Xunit;

namespace Calculator.Core.Tests.Parser;

public class SubformulaParserTest
{
    private SubformulaParser parser = new();

    [Fact]
    public void TryParse_SubformulaAtBeginning_Correct()
    {
        Token token;
        this.parser.TryParse("(1+2)-4", out token);

        this.AssertSubformulaTokenEqual(token, "1+2");
    }

    [Fact]
    public void TryParse_SubformulaAtStartIndex_Correct()
    {
        Token token;
        this.parser.TryParse("1+(2*4)", out token, 2);

        this.AssertSubformulaTokenEqual(token, "2*4");
    }

    [Fact]
    public void TryParse_SubformulaNotAtBeginning_Null()
    {
        Token token;
        this.parser.TryParse("1+(2*4)", out token);

        Assert.Null(token);
    }

    [Fact]
    public void TryParse_SubformulaNotAtStartIndex_Null()
    {
        Token token;
        this.parser.TryParse("1+(2*4)", out token, 4);

        Assert.Null(token);
    }

    [Fact]
    public void TryParse_DifferentParenthesis_Correct()
    {
        Token token;

        this.parser.TryParse("[0-9]", out token);
        Assert.NotNull(token);
        this.AssertSubformulaTokenEqual(token, "0-9");

        this.parser.TryParse("{7+3}", out token);
        Assert.NotNull(token);
        this.AssertSubformulaTokenEqual(token, "7+3");

        this.parser.TryParse("<1+3>*4", out token);
        Assert.NotNull(token);
        this.AssertSubformulaTokenEqual(token, "1+3");
    }

    [Fact]
    public void TryParse_NoClosingParenthesis_Null()
    {
        Token token;

        this.parser.TryParse("(1-2", out token);
        Assert.Null(token);
    }

    [Fact]
    public void TryParse_ClosingParenthesisOfDifferentType_Null()
    {
        Token token;

        this.parser.TryParse("<3}", out token);
        Assert.Null(token);
    }

    [Fact]
    public void TryParse_CrossingParenthesisGroupsOfDifferentTypes_Null()
    {
        Token token;

        this.parser.TryParse("(1+<3-4)*1>", out token);
        Assert.Null(token);
    }

    [Fact]
    public void TryParse_EmptyString_Null()
    {
        Token token;

        this.parser.TryParse(string.Empty, out token);
        Assert.Null(token);
    }

    private void AssertSubformulaTokenEqual(Token token, string value)
    {
        Assert.Equal(TokenType.Subformula, token.Type);
        Assert.Equal(value, token.GetValue());
    }
}