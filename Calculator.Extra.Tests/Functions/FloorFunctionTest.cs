using System;

namespace Calculator.Extra.Tests.Functions;

public class FloorFunctionTest
{
    private readonly Core.Calculator calculator;

    public FloorFunctionTest(Core.Calculator calculator)
    {
        this.calculator = calculator;
    }

    [Fact]
    public void Positive()
    {
        Assert.Equal(2m, this.calculator.Calculate<decimal>("floor(2.4)"));
    }


    [Fact]
    public void Negative()
    {
        Assert.Equal(-1m, this.calculator.Calculate<decimal>("floor(-0.5)"));
    }


    [Fact]
    public void SeveralArguments()
    {
        Assert.Throws<ArgumentException>(() => this.calculator.Calculate<decimal>("floor(4, 1.5)"));
    }
}