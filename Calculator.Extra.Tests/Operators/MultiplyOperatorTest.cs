namespace Calculator.Extra.Tests.Operators;

public class MultiplyOperatorTest
{
    private readonly Core.Calculator calculator;

    public MultiplyOperatorTest(Core.Calculator calculator)
    {
        this.calculator = calculator;
    }

    [Theory]
    [InlineData("3 * 2", 6)]
    [InlineData("1.4 * 6.7", 9.38)]
    [InlineData("1.2 * 5", 6)]
    public void Calculated(string formula, decimal expected)
    {
        Assert.Equal(expected, this.calculator.Calculate<decimal>(formula));
    }
}