using System;
using System.Collections.Generic;
using Calculator.Core.Enum;
using Calculator.Core.Exception;
using Xunit;

namespace Calculator.Core.Tests;

public class CalculatorTest
{
    [Fact]
    public void Calculate_EmptyFormula_Zero()
    {
        Assert.Throws<ArgumentNullException>(() => Calculator.Calculate(string.Empty));
    }

    [Fact]
    public void Calculate_SimpleNumbersSum_Calculated()
    {
        Assert.Equal(3, Calculator.Calculate("1 + 2"));
        Assert.Equal(4, Calculator.Calculate("1.5 + 2.5"));
    }

    [Fact]
    public void Calculate_SimpleNumbersSubtraction_Calculated()
    {
        Assert.Equal(2, Calculator.Calculate("4 - 2"));
        Assert.Equal(2.3m, Calculator.Calculate("10.4 - 8.1"));
    }

    [Fact]
    public void Calculate_PeriodWithNumbers_MeansZeroPeriodNumber()
    {
        Assert.Equal(2, Calculator.Calculate(".5 + 1.5"));
        Assert.Equal(1, Calculator.Calculate("1 - ."));
    }

    [Fact]
    public void Calculate_SimpleNumbersMultiplication_Calculated()
    {
        Assert.Equal(6, Calculator.Calculate("3 * 2"));
        Assert.Equal(9.38m, Calculator.Calculate("1.4 * 6.7"));
    }

    [Fact]
    public void Calculate_SimpleNumbersDivision_Calculated()
    {
        Assert.Equal(1.5m, Calculator.Calculate("3 / 2"));
        Assert.Equal(7, Calculator.Calculate("14 / 2"));
    }

    [Fact]
    public void Calculate_OperationsWithDifferentPriorities_Calculated()
    {
        Assert.Equal(6, Calculator.Calculate("2 + 2 * 2"));
        Assert.Equal(0, Calculator.Calculate("3 / 3 - 1"));
        Assert.Equal(5, Calculator.Calculate("7 - 4 / 2"));
    }

    [Fact]
    public void Calculate_SimpleDivisionByZero_ThrowsException()
    {
        Assert.Throws<DivideByZeroException>(() => Calculator.Calculate("1 / 0"));
    }

    [Fact]
    public void Calculate_SeveralOperationsWithSamePriority_LeftToRightOrder()
    {
        Assert.Equal(2m, Calculator.Calculate("2 - 2 + 2"));
        Assert.Equal(9m, Calculator.Calculate("3 / 1 * 3"));
        Assert.Equal(2m, Calculator.Calculate("2 + 2 - 2"));
        Assert.Equal(3m, Calculator.Calculate("3 * 1 / 3 * 3"));
    }

    [Fact]
    public void Calculate_Parenthesis_OverridesPriority()
    {
        Assert.Equal(8, Calculator.Calculate("(2 + 2) * 2"));
        Assert.Equal(0.5m, Calculator.Calculate("3 / (3 + 3)"));
    }

    [Fact]
    public void Calculate_UnaryPlusMinusInBeginning_Calculated()
    {
        Assert.Equal(-1, Calculator.Calculate("-1"));
        Assert.Equal(3, Calculator.Calculate("+3"));
        Assert.Equal(1, Calculator.Calculate("-1 + 2"));
    }

    [Fact]
    public void Calculate_UnaryPlusMinusInParenthesis_Calculated()
    {
        Assert.Equal(-3, Calculator.Calculate("2 + (-5)"));
        Assert.Equal(1, Calculator.Calculate("(-2) + (4 - 1)"));
        Assert.Equal(0.5m, Calculator.Calculate("1 / (+2)"));
        Assert.Equal(4, Calculator.Calculate("(+4) * 1"));
    }

    [Fact]
    public void Calculate_Floor_Calculated()
    {
        Assert.Equal(2m, Calculator.Calculate("floor(2.4)"));
        Assert.Equal(-1m, Calculator.Calculate("floor(-0.5)"));
        Assert.Equal(3, Calculator.Calculate("1 + floor(4 / 1.5)"));
    }

    [Fact]
    public void Calculate_Ceil_Calculated()
    {
        Assert.Equal(3m, Calculator.Calculate("ceil(2.4)"));
        Assert.Equal(0m, Calculator.Calculate("ceil(-0.5)"));
        Assert.Equal(-1, Calculator.Calculate("1 + ceil(-4 / 1.5)"));
    }

    [Fact]
    public void Calculate_OrOperation_Calculate()
    {
        Assert.Equal(false, Calculator.Calculate<bool>("false || false"));
        Assert.Equal(true, Calculator.Calculate<bool>("false || true"));
        Assert.Equal(true, Calculator.Calculate<bool>("true || false"));
        Assert.Equal(true, Calculator.Calculate<bool>("true || true"));
    }

    [Fact]
    public void Calculate_AndOperation_Calculate()
    {
        Assert.Equal(false, Calculator.Calculate<bool>("false && false"));
        Assert.Equal(false, Calculator.Calculate<bool>("false && true"));
        Assert.Equal(false, Calculator.Calculate<bool>("true && false"));
        Assert.Equal(true, Calculator.Calculate<bool>("true && true"));
    }

    [Fact]
    public void Calculate_NotOperation_Calculate()
    {
        Assert.Equal(false, Calculator.Calculate<bool>("!true"));
        Assert.Equal(true, Calculator.Calculate<bool>("!false"));
    }

    [Fact]
    public void Calculate_LogicalOperationsPriority_Calculate()
    {
        Assert.Equal(false, Calculator.Calculate<bool>("!true && false"));
        Assert.Equal(true, Calculator.Calculate<bool>("!false || true"));
    }

    [Fact]
    public void Calculate_DifferentParenthesis_Calculated()
    {
        Assert.Equal(2.5m, Calculator.Calculate<decimal>("< 2 + 3 > * (1 - { 3 / 6 })"));
    }

    [Fact]
    public void Calculate_OneVariable_Calculated()
    {
        Assert.Equal(
            4.5m,
            Calculator.Calculate(
                "var1",
                new Dictionary<string, object>()
                {
                    { "var1", 4.5m }
                }
            )
        );
        Assert.Equal(
            true,
            Calculator.Calculate<bool>(
                "testVar",
                new Dictionary<string, object>()
                {
                    { "testVar", true }
                }
            )
        );
    }

    [Fact]
    public void Calculate_VariableAndNumber_Calculated()
    {
        Assert.Equal(
            7.5m,
            Calculator.Calculate(
                "2.5 * my_var",
                new Dictionary<string, object>()
                {
                    { "my_var", 3 }
                }
            )
        );
    }

    [Fact]
    public void Calculate_UnmatchedClosingParenthesis_ThrowsParseException()
    {
        ParseException exception = Assert.Throws<ParseException>(() => Calculator.Calculate(")+3"));
        Assert.Equal((int)ParseExceptionCode.UnparsedToken, exception.Code);
    }

    [Fact]
    public void Calculate_UnknownOperation_ThrowsCalculateException()
    {
        CalculateException exception = Assert.Throws<CalculateException>(() => Calculator.Calculate("1+-2"));
        Assert.Equal((int)CalculateExceptionCode.UnknownOperation, exception.Code);
    }

    [Fact]
    public void Calculate_SeveralResults_ThrowsCalculateException()
    {
        CalculateException exception = Assert.Throws<CalculateException>(() => Calculator.Calculate("(1)(2)(3)"));
        Assert.Equal((int)CalculateExceptionCode.NotSingleResult, exception.Code);
    }

    [Fact]
    public void Calculate_BinaryOperationMissingLeftOperand_ThrowsException()
    {
        CalculateException exception = Assert.Throws<CalculateException>(() => Calculator.Calculate("*2"));
        Assert.Equal((int)CalculateExceptionCode.MissingOperand, exception.Code);
    }

    [Fact]
    public void Calculate_BinaryOperationMissingBothOperands_ThrowsException()
    {
        CalculateException exception = Assert.Throws<CalculateException>(() => Calculator.Calculate("/"));
        Assert.Equal((int)CalculateExceptionCode.MissingOperand, exception.Code);
    }

    [Fact]
    public void Calculate_UnaryOperationMissingOperand_ThrowsException()
    {
        CalculateException exception = Assert.Throws<CalculateException>(() => Calculator.Calculate("+"));
        Assert.Equal((int)CalculateExceptionCode.MissingOperand, exception.Code);
    }

    [Fact]
    public void Calculate_NoClosingParenthesis_ThrowsException()
    {
        ParseException exception = Assert.Throws<ParseException>(() => Calculator.Calculate("2*(3-4"));
        Assert.Equal((int)ParseExceptionCode.UnparsedToken, exception.Code);
    }

    [Fact]
    public void Calculate_ClosingParenthesisOfDifferentType_ThrowsException()
    {
        ParseException exception = Assert.Throws<ParseException>(() => Calculator.Calculate("2*(3-4>/4"));
        Assert.Equal((int)ParseExceptionCode.UnparsedToken, exception.Code);
    }

    [Fact]
    public void Calculate_UndefinedVariable_ThrowsException()
    {
        CalculateException exception = Assert.Throws<CalculateException>(() => Calculator.Calculate("a"));
        Assert.Equal((int)CalculateExceptionCode.UnknownVariable, exception.Code);
    }

    [Fact]
    public void Calculate_StringVariable_ThrowsException()
    {
        CalculateException exception = Assert.Throws<CalculateException>(() => Calculator.Calculate(
            "a + c",
            new Dictionary<string, object>()
            {
                { "a", 1 },
                { "c", "test" }
            }
        ));
        Assert.Equal((int)CalculateExceptionCode.StringVariable, exception.Code);
    }

    [Fact]
    public void Calculate_OperationVariable_ThrowsException()
    {
        CalculateException exception = Assert.Throws<CalculateException>(() => Calculator.Calculate(
            "c / 0",
            new Dictionary<string, object>()
            {
                { "c", "-" }
            }
        ));
        Assert.Equal((int)CalculateExceptionCode.StringVariable, exception.Code);
    }

    [Fact]
    public void Calculate_SubformulaVariable_ThrowsException()
    {
        CalculateException exception = Assert.Throws<CalculateException>(() => Calculator.Calculate(
            "v",
            new Dictionary<string, object>()
            {
                { "v", "(1+2)" }
            }
        ));
        Assert.Equal((int)CalculateExceptionCode.StringVariable, exception.Code);
    }
}