using System;

namespace Calculator.Extra.Tests.Operators;

public class DivideOperatorTest
{
    private readonly Core.Calculator calculator;

    public DivideOperatorTest(Core.Calculator calculator)
    {
        this.calculator = calculator;
    }

    [Theory]
    [InlineData("3 / 2", 1.5)]
    [InlineData("14 / 2", 7)]
    [InlineData("14 / 2 / 4", 1.75)]
    [InlineData("14 / (2 / 4)", 28)]
    public void Calculated(string formula, decimal expected)
    {
        Assert.Equal(expected, this.calculator.Calculate<decimal>(formula));
    }

    [Fact]
    public void Calculate_SimpleDivisionByZero_ThrowsException()
    {
        Assert.Throws<DivideByZeroException>(() => this.calculator.Calculate<decimal>("1 / 0"));
    }
}