namespace Calculator.Extra.Tests.Operators;

public class AddOperatorTest
{
    private readonly Core.Calculator calculator;

    public AddOperatorTest(Core.Calculator calculator)
    {
        this.calculator = calculator;
    }

    [Theory]
    [InlineData("1 + 2", 3)]
    [InlineData("1.5 + 2.5", 4)]
    [InlineData("1.5 + 2.5 + 7", 11)]
    public void Calculated(string formula, decimal expected)
    {
        Assert.Equal(expected, this.calculator.Calculate<decimal>(formula));
    }

    [Fact]
    public void UnaryInBeginning()
    {
        Assert.Equal(3, this.calculator.Calculate<decimal>("+3"));
    }

    [Fact]
    public void UnaryInParenthesis()
    {
        Assert.Equal(7, this.calculator.Calculate<decimal>("2 + (+5)"));
    }
}