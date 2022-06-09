using Calculator.Core.Operands;
using Calculator.Core.ParsingContexts;
using Calculator.Core.Tokens;
using Calculator.Extra.Parsers;

namespace Calculator.Extra.Tests.Parsers;

public class DecimalOperandParserTest
{
    private readonly DecimalOperandParser parser = new();
    private readonly ParsingContext context = ParsingContext.Initial;

    [Fact]
    public void TryParse_NumberAtBeginning_CorrectNumber()
    {
        bool isParsed = this.parser.TryParse("13.9 - 7", this.context, out Token? token, out _);

        Assert.True(isParsed);
        this.AssertDecimalTokenEqual(token, 13.9m);
    }

    [Fact]
    public void TryParse_NumberNotAtBeginning_Null()
    {
        bool isParsed = this.parser.TryParse("-1", this.context, out Token? token, out _);

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

    [Theory]
    [InlineData(".34", 0.34)]
    [InlineData(".", 0)]
    public void GetTokens_NumberWithLeadingPeriod_CorrectNumberToken(string formula, decimal expectedValue)
    {
        bool isParsed = this.parser.TryParse(formula, this.context, out Token? token, out _);

        Assert.True(isParsed);
        this.AssertDecimalTokenEqual(token, expectedValue);
    }

    private void AssertDecimalTokenEqual(Token? token, decimal value)
    {
        Assert.NotNull(token);
        Operand<decimal> decimalToken = Assert.IsType<Operand<decimal>>(token);
        Assert.Equal(value, decimalToken.Value);
    }
}