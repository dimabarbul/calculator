using System;

namespace Calculator.Extra.Tests.Functions;

public class CeilFunctionTest
{
    private readonly Core.Calculator calculator;

    public CeilFunctionTest(Core.Calculator calculator)
    {
        this.calculator = calculator;
    }

    [Fact]
    public void PositiveNumber()
    {
        Assert.Equal(3m, this.calculator.Calculate<decimal>("ceil(2.4)"));
    }

    [Fact]
    public void NegativeNumber()
    {
        Assert.Equal(0m, this.calculator.Calculate<decimal>("ceil(-0.5)"));
    }

    [Fact]
    public void MultipleArguments()
    {
        Assert.Throws<ArgumentException>(() => this.calculator.Calculate<decimal>("ceil(-4, 1.5)"));
    }
}