using Calculator.Core.Enum;
using Xunit;

namespace Calculator.Core.Tests;

public class TokenTest
{
    [Fact]
    public void GetValueDecimal_Number_CorrectValue()
    {
        Token token = new("2", TokenType.Decimal);

        Assert.Equal(2m, token.GetValue<decimal>());
    }

    [Fact]
    public void GetValueDecimal_DecimalWithLeadingPeriod_CorrectValue()
    {
        Token token = new(".3", TokenType.Decimal);

        Assert.Equal(0.3m, token.GetValue<decimal>());
    }

    [Fact]
    public void GetValue_WithTypeDecimal_Decimal()
    {
        Token token = new("4", TokenType.Decimal);

        dynamic value = token.GetValue();
        Assert.Equal(typeof(decimal), value.GetType());
        Assert.Equal(4m, value);
    }
}