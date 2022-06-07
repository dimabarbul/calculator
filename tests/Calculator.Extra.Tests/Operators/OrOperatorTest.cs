namespace Calculator.Extra.Tests.Operators;

public class OrOperatorTest
{
    private readonly Core.Calculator calculator;

    public OrOperatorTest(Core.Calculator calculator)
    {
        this.calculator = calculator;
    }

    [Theory]
    [InlineData("false || false", false)]
    [InlineData("false || true", true)]
    [InlineData("true || false", true)]
    [InlineData("true || true", true)]
    public void Calculated(string formula, bool expected)
    {
        Assert.Equal(expected, this.calculator.Calculate<bool>(formula));
    }
}