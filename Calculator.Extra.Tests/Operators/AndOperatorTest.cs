namespace Calculator.Extra.Tests.Operators;

public class AndOperatorTest
{
    private readonly Core.Calculator calculator;

    public AndOperatorTest(Core.Calculator calculator)
    {
        this.calculator = calculator;
    }

    [Theory]
    [InlineData("false && false", false)]
    [InlineData("false && true", false)]
    [InlineData("true && false", false)]
    [InlineData("true && true", true)]
    public void Calculated(string formula, bool expected)
    {
        Assert.Equal(expected, this.calculator.Calculate<bool>(formula));
    }
}