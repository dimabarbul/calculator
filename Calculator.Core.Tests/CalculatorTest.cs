using System;
using System.Collections.Generic;
using Calculator.Core.Enum;
using Calculator.Core.Exception;
using Calculator.Core.Operands;
using Xunit;

namespace Calculator.Core.Tests;

public class CalculatorTest
{
    private readonly Calculator calculator;

    public CalculatorTest(Calculator calculator)
    {
        this.calculator = calculator;
    }

    [Fact]
    public void Calculate_EmptyFormula_Zero()
    {
        Assert.Throws<ArgumentNullException>(() => this.calculator.Calculate<decimal>(string.Empty));
    }

    [Fact]
    public void Calculate_SimpleNumbersSum_Calculated()
    {
        Assert.Equal(3, this.calculator.Calculate<decimal>("1 + 2"));
        Assert.Equal(4, this.calculator.Calculate<decimal>("1.5 + 2.5"));
    }

    [Fact]
    public void Calculate_SimpleNumbersSubtraction_Calculated()
    {
        Assert.Equal(2, this.calculator.Calculate<decimal>("4 - 2"));
        Assert.Equal(2.3m, this.calculator.Calculate<decimal>("10.4 - 8.1"));
    }

    [Fact]
    public void Calculate_PeriodWithNumbers_MeansZeroPeriodNumber()
    {
        Assert.Equal(2, this.calculator.Calculate<decimal>(".5 + 1.5"));
        Assert.Equal(1, this.calculator.Calculate<decimal>("1 - ."));
    }

    [Fact]
    public void Calculate_SimpleNumbersMultiplication_Calculated()
    {
        Assert.Equal(6, this.calculator.Calculate<decimal>("3 * 2"));
        Assert.Equal(9.38m, this.calculator.Calculate<decimal>("1.4 * 6.7"));
    }

    [Fact]
    public void Calculate_SimpleNumbersDivision_Calculated()
    {
        Assert.Equal(1.5m, this.calculator.Calculate<decimal>("3 / 2"));
        Assert.Equal(7, this.calculator.Calculate<decimal>("14 / 2"));
    }

    [Fact]
    public void Calculate_OperationsWithDifferentPriorities_Calculated()
    {
        Assert.Equal(6, this.calculator.Calculate<decimal>("2 + 2 * 2"));
        Assert.Equal(0, this.calculator.Calculate<decimal>("3 / 3 - 1"));
        Assert.Equal(5, this.calculator.Calculate<decimal>("7 - 4 / 2"));
    }

    [Fact]
    public void Calculate_SimpleDivisionByZero_ThrowsException()
    {
        Assert.Throws<DivideByZeroException>(() => this.calculator.Calculate<decimal>("1 / 0"));
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
    public void Calculate_Parenthesis_OverridesPriority()
    {
        Assert.Equal(8, this.calculator.Calculate<decimal>("(2 + 2) * 2"));
        Assert.Equal(0.5m, this.calculator.Calculate<decimal>("3 / (3 + 3)"));
    }

    [Fact]
    public void Calculate_UnaryPlusMinusInBeginning_Calculated()
    {
        Assert.Equal(-1, this.calculator.Calculate<decimal>("-1"));
        Assert.Equal(3, this.calculator.Calculate<decimal>("+3"));
        Assert.Equal(1, this.calculator.Calculate<decimal>("-1 + 2"));
    }

    [Fact]
    public void Calculate_UnaryPlusMinusInParenthesis_Calculated()
    {
        Assert.Equal(-3, this.calculator.Calculate<decimal>("2 + (-5)"));
        Assert.Equal(1, this.calculator.Calculate<decimal>("(-2) + (4 - 1)"));
        Assert.Equal(0.5m, this.calculator.Calculate<decimal>("1 / (+2)"));
        Assert.Equal(4, this.calculator.Calculate<decimal>("(+4) * 1"));
    }

    [Fact]
    public void Calculate_Floor_Calculated()
    {
        Assert.Equal(2m, this.calculator.Calculate<decimal>("floor(2.4)"));
        Assert.Equal(-1m, this.calculator.Calculate<decimal>("floor(-0.5)"));
        Assert.Equal(3, this.calculator.Calculate<decimal>("1 + floor(4 / 1.5)"));
    }

    [Fact]
    public void Calculate_Ceil_Calculated()
    {
        Assert.Equal(3m, this.calculator.Calculate<decimal>("ceil(2.4)"));
        Assert.Equal(0m, this.calculator.Calculate<decimal>("ceil(-0.5)"));
        Assert.Equal(-1, this.calculator.Calculate<decimal>("1 + ceil(-4 / 1.5)"));
    }

    [Fact]
    public void Calculate_OrOperation_Calculate()
    {
        Assert.Equal(false, this.calculator.Calculate<bool>("false || false"));
        Assert.Equal(true, this.calculator.Calculate<bool>("false || true"));
        Assert.Equal(true, this.calculator.Calculate<bool>("true || false"));
        Assert.Equal(true, this.calculator.Calculate<bool>("true || true"));
    }

    [Fact]
    public void Calculate_AndOperation_Calculate()
    {
        Assert.Equal(false, this.calculator.Calculate<bool>("false && false"));
        Assert.Equal(false, this.calculator.Calculate<bool>("false && true"));
        Assert.Equal(false, this.calculator.Calculate<bool>("true && false"));
        Assert.Equal(true, this.calculator.Calculate<bool>("true && true"));
    }

    [Fact]
    public void Calculate_NotOperation_Calculate()
    {
        Assert.Equal(false, this.calculator.Calculate<bool>("!true"));
        Assert.Equal(true, this.calculator.Calculate<bool>("!false"));
    }

    [Fact]
    public void Calculate_LogicalOperationsPriority_Calculate()
    {
        Assert.Equal(false, this.calculator.Calculate<bool>("!true && false"));
        Assert.Equal(true, this.calculator.Calculate<bool>("!false || true"));
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
        Assert.Equal(
            true,
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
    public void Calculate_UnmatchedClosingParenthesis_ThrowsParseException()
    {
        ParseException exception = Assert.Throws<ParseException>(() => this.calculator.Calculate<decimal>(")+3"));
        Assert.Equal((int)ParseExceptionCode.UnparsedToken, exception.Code);
    }

    [Fact]
    public void Calculate_SeveralResults_ThrowsCalculateException()
    {
        CalculateException exception = Assert.Throws<CalculateException>(() => this.calculator.Calculate<decimal>("(1)(2)(3)"));
        Assert.Equal((int)CalculateExceptionCode.NotSingleResult, exception.Code);
    }

    [Fact]
    public void Calculate_BinaryOperationMissingLeftOperand_ThrowsException()
    {
        CalculateException exception = Assert.Throws<CalculateException>(() => this.calculator.Calculate<decimal>("*2"));
        Assert.Equal((int)CalculateExceptionCode.MissingOperand, exception.Code);
    }

    [Fact]
    public void Calculate_BinaryOperationMissingBothOperands_ThrowsException()
    {
        CalculateException exception = Assert.Throws<CalculateException>(() => this.calculator.Calculate<decimal>("/"));
        Assert.Equal((int)CalculateExceptionCode.MissingOperand, exception.Code);
    }

    [Fact]
    public void Calculate_UnaryOperationMissingOperand_ThrowsException()
    {
        CalculateException exception = Assert.Throws<CalculateException>(() => this.calculator.Calculate<decimal>("+"));
        Assert.Equal((int)CalculateExceptionCode.MissingOperand, exception.Code);
    }

    [Fact]
    public void Calculate_NoClosingParenthesis_ThrowsException()
    {
        ParseException exception = Assert.Throws<ParseException>(() => this.calculator.Calculate<decimal>("2*(3-4"));
        Assert.Equal((int)ParseExceptionCode.UnparsedToken, exception.Code);
    }

    [Fact]
    public void Calculate_ClosingParenthesisOfDifferentType_ThrowsException()
    {
        ParseException exception = Assert.Throws<ParseException>(() => this.calculator.Calculate<decimal>("2*(3-4>/4"));
        Assert.Equal((int)ParseExceptionCode.UnparsedToken, exception.Code);
    }

    [Fact]
    public void Calculate_UndefinedVariable_ThrowsException()
    {
        CalculateException exception = Assert.Throws<CalculateException>(() => this.calculator.Calculate<decimal>("$a"));
        Assert.Equal((int)CalculateExceptionCode.UnknownVariable, exception.Code);
    }

    [Fact]
    public void Calculate_ListOperand_Correct()
    {
        Token token = this.calculator.Calculate("(1, 2, 3)");

        ListOperand listOperand = Assert.IsType<ListOperand>(token);
        Operand<decimal> decimalOperand = Assert.IsType<Operand<decimal>>(listOperand.Operands[0]);
        Assert.Equal(1, decimalOperand.Value);

        decimalOperand = Assert.IsType<Operand<decimal>>(listOperand.Operands[1]);
        Assert.Equal(2, decimalOperand.Value);

        decimalOperand = Assert.IsType<Operand<decimal>>(listOperand.Operands[2]);
        Assert.Equal(3, decimalOperand.Value);
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

    [Theory]
    [InlineData("max(1, 2, 3)", 3)]
    [InlineData("max((-1), (-2), (-3))", -1)]
    [InlineData("max((-9), 7)", 7)]
    public void Calculate_MaxFunction_Correct(string formula, decimal expected)
    {
        decimal actual = this.calculator.Calculate<decimal>(formula);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("1+-1")]
    [InlineData("1-+1")]
    public void Calculate_UnaryOperatorInTheMiddleOfFormula_ThrowsException(string formula)
    {
        CalculateException exception = Assert.Throws<CalculateException>(() => this.calculator.Calculate<decimal>(
            formula
        ));
        Assert.Equal((int)CalculateExceptionCode.SubsequentOperators, exception.Code);
    }
}