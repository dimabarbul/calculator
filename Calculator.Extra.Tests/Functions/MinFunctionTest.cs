namespace Calculator.Extra.Tests.Functions;

public class MinFunctionTest
{
    private readonly Core.Calculator calculator;

    public MinFunctionTest(Core.Calculator calculator)
    {
        this.calculator = calculator;
    }

    [Theory]
    [InlineData("min(1, 2, 3)", 1)]
    [InlineData("min((-1), (-2), (-3))", -3)]
    [InlineData("min((-7), 9)", -7)]
    public void Calculate_MinFunction_Correct(string formula, decimal expected)
    {
        decimal actual = this.calculator.Calculate<decimal>(formula);

        Assert.Equal(expected, actual);
    }
}