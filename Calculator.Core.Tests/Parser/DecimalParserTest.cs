using Calculator.Core.Enum;
using Calculator.Core.Parser;
using Xunit;

namespace Calculator.Core.Tests.Parser;

public class DecimalParserTest
{
    private DecimalParser parser = new();

    [Fact]
    public void TryParse_NumberAtBeginning_CorrectNumber()
    {
        Token? token;
        this.parser.TryParse("13.9 - 7", out token, out _);

        this.AssertDecimalTokenEqual(token, 13.9m);
    }

    [Fact]
    public void TryParse_NumberNotAtBeginning_Null()
    {
        Token? token;
        this.parser.TryParse("-1", out token, out _);

        Assert.Null(token);
    }

    [Fact]
    public void TryParse_EmptyString_Null()
    {
        Token? token;

        this.parser.TryParse(string.Empty, out token, out _);
        Assert.Null(token);
    }

    private void AssertDecimalTokenEqual(Token? token, decimal value)
    {
        Assert.NotNull(token);
        Assert.Equal(TokenType.Decimal, token.Type);
        Assert.Equal(value, token.GetValue());
    }
}