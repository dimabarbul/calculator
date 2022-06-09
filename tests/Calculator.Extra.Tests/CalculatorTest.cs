using System.Collections.Generic;
using Calculator.Core.Operands;
using Calculator.Core.Tokens;

namespace Calculator.Extra.Tests;

public class CalculatorTest
{
    private readonly Core.Calculator calculator;

    public CalculatorTest(Core.Calculator calculator)
    {
        this.calculator = calculator;
    }

    [Fact]
    public void Calculate_SeveralOperationsWithSamePriority_LeftToRightOrder()
    {
        Assert.Equal(2m, this.calculator.Calculate<decimal>("2 - 2 + 2"));
        Assert.Equal(9m, this.calculator.Calculate<decimal>("3 / 1 * 3"));
        Assert.Equal(2m, this.calculator.Calculate<decimal>("2 + 2 - 2"));
        Assert.Equal(3m, this.calculator.Calculate<decimal>("3 * 1 / 3 * 3"));
    }

    [Fact]
    public void Calculate_LogicalOperationsPriority_Calculate()
    {
        Assert.False(this.calculator.Calculate<bool>("!true && false"));
        Assert.True(this.calculator.Calculate<bool>("!false || true"));
        Assert.True(this.calculator.Calculate<bool>("true || true && false"));
    }

    [Fact]
    public void Calculate_DifferentParenthesis_Calculated()
    {
        Assert.Equal(2.5m, this.calculator.Calculate<decimal>("< 2 + 3 > * (1 - { 3 / 6 })"));
    }

    [Fact]
    public void Calculate_OneVariable_Calculated()
    {
        Assert.Equal(
            4.5m,
            this.calculator.Calculate<decimal>(
                "$var1",
                new Dictionary<string, Operand>
                {
                    { "var1", new Operand<decimal>(4.5m) }
                }
            )
        );
        Assert.True(
            this.calculator.Calculate<bool>(
                "$testVar",
                new Dictionary<string, Operand>
                {
                    { "testVar", new Operand<bool>(true) }
                }
            )
        );
    }

    [Fact]
    public void Calculate_VariableAndNumber_Calculated()
    {
        Assert.Equal(
            7.5m,
            this.calculator.Calculate<decimal>(
                "2.5 * $my_var",
                new Dictionary<string, Operand>
                {
                    { "my_var", new Operand<decimal>(3) }
                }
            )
        );
    }

    [Fact]
    public void Calculate_OperatorAndFunction_Calculated()
    {
        decimal result = this.calculator.Calculate<decimal>("1 + ceil(1.5)");

        Assert.Equal(3, result);
    }

    [Fact]
    public void Calculate_LogicalOperatorWithNot_Calculated()
    {
        bool result = this.calculator.Calculate<bool>("true && !false");

        Assert.True(result);
    }
}