namespace Calculator.Extra.Tests.Operators;

public class NotOperatorTest
{
    private readonly Core.Calculator calculator;

    public NotOperatorTest(Core.Calculator calculator)
    {
        this.calculator = calculator;
    }

    [Theory]
    [InlineData("!false", true)]
    [InlineData("!true", false)]
    public void Calculated(string formula, bool expected)
    {
        Assert.Equal(expected, this.calculator.Calculate<bool>(formula));
    }
}