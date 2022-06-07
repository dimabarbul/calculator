namespace Calculator.Extra.Tests.Operators;

public class SubtractOperatorTest
{
    private readonly Core.Calculator calculator;

    public SubtractOperatorTest(Core.Calculator calculator)
    {
        this.calculator = calculator;
    }

    [Theory]
    [InlineData("4 - 2", 2)]
    [InlineData("10.4 - 8.1", 2.3)]
    [InlineData("10 - 5.6 - 8.4 - 1", -5)]
    public void Calculated(string formula, decimal expected)
    {
        Assert.Equal(expected, this.calculator.Calculate<decimal>(formula));
    }

    [Fact]
    public void UnaryInBeginning()
    {
        Assert.Equal(-1, this.calculator.Calculate<decimal>("-1"));
    }

    [Fact]
    public void UnaryInParenthesis()
    {
        Assert.Equal(7, this.calculator.Calculate<decimal>("2 - (-5)"));
    }
}