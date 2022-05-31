using Calculator.Core.Operands;
using Calculator.Core.Parser;
using Xunit;

namespace Calculator.Core.Tests.Parser;

public class BoolParserTest
{
    private BoolParser parser = new();

    [Fact]
    public void TryParse_TrueAtBeginning_Correct()
    {
        Token? token;
        this.parser.TryParse("true", out token, out _);

        this.AssertBoolTokenEqual(token, true);
    }

    [Fact]
    public void TryParse_FalseAtBeginning_Correct()
    {
        Token? token;
        this.parser.TryParse("false", out token, out _);

        this.AssertBoolTokenEqual(token, false);
    }

    [Fact]
    public void TryParse_BoolNotAtBeginning_Null()
    {
        Token? token;
        this.parser.TryParse("1||false", out token, out _);

        Assert.Null(token);
    }

    [Fact]
    public void TryParse_EmptyString_Null()
    {
        Token? token;

        this.parser.TryParse(string.Empty, out token, out _);
        Assert.Null(token);
    }

    private void AssertBoolTokenEqual(Token? token, bool value)
    {
        Assert.NotNull(token);
        Operand<bool> boolToken = Assert.IsType<Operand<bool>>(token);
        Assert.Equal(value, boolToken.Value);
    }
}