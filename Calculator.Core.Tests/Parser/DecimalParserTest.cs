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
        Token token;
        this.parser.TryParse("13.9 - 7", out token);

        this.AssertDecimalTokenEqual(token, 13.9m);
    }

    [Fact]
    public void TryParse_NumberAtStartIndex_CorrectNumber()
    {
        Token token;
        this.parser.TryParse("1+2", out token, 2);

        this.AssertDecimalTokenEqual(token, 2m);
    }

    [Fact]
    public void TryParse_NumberNotAtBeginning_Null()
    {
        Token token;
        this.parser.TryParse("-1", out token);

        Assert.Null(token);
    }

    [Fact]
    public void TryParse_NumberNotAtStartIndex_Null()
    {
        Token token;
        this.parser.TryParse("1+2", out token, 1);

        Assert.Null(token);
    }

    [Fact]
    public void TryParse_EmptyString_Null()
    {
        Token token;

        this.parser.TryParse(string.Empty, out token);
        Assert.Null(token);
    }

    private void AssertDecimalTokenEqual(Token token, decimal value)
    {
        Assert.Equal(TokenType.Decimal, token.Type);
        Assert.Equal(value, token.GetValue());
    }
}