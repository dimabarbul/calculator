namespace Calculator.Extra.Tests.Functions;

public class MaxFunctionTest
{
    private readonly Core.Calculator calculator;

    public MaxFunctionTest(Core.Calculator calculator)
    {
        this.calculator = calculator;
    }

    [Theory]
    [InlineData("max(1, 2, 3)", 3)]
    [InlineData("max((-1), (-2), (-3))", -1)]
    [InlineData("max((-9), 7)", 7)]
    public void Calculate_MaxFunction_Correct(string formula, decimal expected)
    {
        decimal actual = this.calculator.Calculate<decimal>(formula);

        Assert.Equal(expected, actual);
    }
}