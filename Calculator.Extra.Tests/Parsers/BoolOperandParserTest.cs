using Calculator.Core.Operands;
using Calculator.Core.Tokens;
using Calculator.Extra.Parsers;

namespace Calculator.Extra.Tests.Parsers;

public class BoolOperandParserTest
{
    private readonly BoolOperandParser parser = new();

    [Fact]
    public void TryParse_TrueAtBeginning_Correct()
    {
        bool isParsed = this.parser.TryParse("true", out Token? token, out _);

        Assert.True(isParsed);
        this.AssertBoolTokenEqual(token, true);
    }

    [Fact]
    public void TryParse_FalseAtBeginning_Correct()
    {
        bool isParsed = this.parser.TryParse("false", out Token? token, out _);

        Assert.True(isParsed);
        this.AssertBoolTokenEqual(token, false);
    }

    [Fact]
    public void TryParse_BoolNotAtBeginning_Null()
    {
        bool isParsed = this.parser.TryParse("1||false", out Token? token, out _);

        Assert.False(isParsed);
        Assert.Null(token);
    }

    [Fact]
    public void TryParse_EmptyString_Null()
    {
        bool isParsed = this.parser.TryParse(string.Empty, out Token? token, out _);

        Assert.False(isParsed);
        Assert.Null(token);
    }

    private void AssertBoolTokenEqual(Token? token, bool value)
    {
        Assert.NotNull(token);
        Operand<bool> boolToken = Assert.IsType<Operand<bool>>(token);
        Assert.Equal(value, boolToken.Value);
    }
}