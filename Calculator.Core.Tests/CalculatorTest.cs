using System;
using System.Collections.Generic;
using System.Linq;
using Calculator.Core.Enums;
using Calculator.Core.Exceptions;
using Calculator.Core.Operands;
using Calculator.Core.Tests.Extensions;
using Calculator.Core.Tokens;
using Xunit;

namespace Calculator.Core.Tests;

public class CalculatorTest
{
    private readonly Calculator calculator;
    private readonly CalcLowPriorityOperator lowPriorityOperator;
    private readonly CalcHighPriorityOperator highPriorityOperator;

    public CalculatorTest(Calculator calculator, IEnumerable<Operation> operations)
    {
        this.calculator = calculator;

        this.lowPriorityOperator = (CalcLowPriorityOperator)operations.First(o => o is CalcLowPriorityOperator);
        this.highPriorityOperator = (CalcHighPriorityOperator)operations.First(o => o is CalcHighPriorityOperator);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Calculate_EmptyFormula_ThrowsException(string formula)
    {
        Assert.Throws<ArgumentNullException>(() => this.calculator.Calculate<int>(formula));
    }

    [Theory]
    [InlineData("1 low 2", CalcLowPriorityOperator.ReturnValue)]
    [InlineData("1 high 2", CalcHighPriorityOperator.ReturnValue)]
    public void Calculate_OneOperation_Calculated(string formula, int result)
    {
        Assert.Equal(result, this.calculator.Calculate<int>(formula));
    }

    [Fact]
    public void Calculate_DifferentPriorities_CorrectOrder()
    {
        int result = this.calculator.Calculate<int>("1 low 2 high 3");

        Assert.Single(this.highPriorityOperator.Calls);
        Assert.Equal(2, this.highPriorityOperator.Calls[0].Length);
        AssertExtensions.TokenIs<Operand<int>>(
            this.highPriorityOperator.Calls[0][0],
            o => Assert.Equal(2, o.Value));
        AssertExtensions.TokenIs<Operand<int>>(
            this.highPriorityOperator.Calls[0][1],
            o => Assert.Equal(3, o.Value));

        Assert.Single(this.lowPriorityOperator.Calls);
        Assert.Equal(2, this.lowPriorityOperator.Calls[0].Length);
        AssertExtensions.TokenIs<Operand<int>>(
            this.lowPriorityOperator.Calls[0][0],
            o => Assert.Equal(1, o.Value));
        AssertExtensions.TokenIs<Operand<int>>(
            this.lowPriorityOperator.Calls[0][1],
            o => Assert.Equal(CalcHighPriorityOperator.ReturnValue, o.Value));

        Assert.Equal(CalcLowPriorityOperator.ReturnValue, result);
    }

    [Theory]
    [InlineData("(1 low 2) high 3")]
    [InlineData("<1 low 2> high 3")]
    [InlineData("{1 low 2} high 3")]
    [InlineData("[1 low 2] high 3")]
    public void Calculate_Parenthesis_OverridesPriority(string formula)
    {
        int result = this.calculator.Calculate<int>(formula);

        Assert.Single(this.lowPriorityOperator.Calls);
        Assert.Equal(2, this.lowPriorityOperator.Calls[0].Length);
        AssertExtensions.TokenIs<Operand<int>>(
            this.lowPriorityOperator.Calls[0][0],
            o => Assert.Equal(1, o.Value));
        AssertExtensions.TokenIs<Operand<int>>(
            this.lowPriorityOperator.Calls[0][1],
            o => Assert.Equal(2, o.Value));

        Assert.Single(this.highPriorityOperator.Calls);
        Assert.Equal(2, this.highPriorityOperator.Calls[0].Length);
        AssertExtensions.TokenIs<Operand<int>>(
            this.highPriorityOperator.Calls[0][0],
            o => Assert.Equal(CalcLowPriorityOperator.ReturnValue, o.Value));
        AssertExtensions.TokenIs<Operand<int>>(
            this.highPriorityOperator.Calls[0][1],
            o => Assert.Equal(3, o.Value));

        Assert.Equal(CalcHighPriorityOperator.ReturnValue, result);
    }

    [Fact]
    public void Calculate_UnmatchedClosingParenthesis_ThrowsParseException()
    {
        ParseException exception = Assert.Throws<ParseException>(() => this.calculator.Calculate<int>(")"));
        Assert.Equal((int)ParseExceptionCode.UnparsedToken, exception.Code);
    }

    [Fact]
    public void Calculate_SeveralResults_ThrowsCalculateException()
    {
        CalculateException exception = Assert.Throws<CalculateException>(() => this.calculator.Calculate<int>("1 2"));
        Assert.Equal((int)CalculateExceptionCode.NotSingleResult, exception.Code);
    }

    [Fact]
    public void Calculate_BinaryOperationMissingLeftOperand_ThrowsException()
    {
        CalculateException exception = Assert.Throws<CalculateException>(() => this.calculator.Calculate<int>("low 2"));
        Assert.Equal((int)CalculateExceptionCode.MissingOperand, exception.Code);
    }

    [Fact]
    public void Calculate_BinaryOperationMissingBothOperands_ThrowsException()
    {
        CalculateException exception = Assert.Throws<CalculateException>(() => this.calculator.Calculate<int>("high"));
        Assert.Equal((int)CalculateExceptionCode.MissingOperand, exception.Code);
    }

    [Fact]
    public void Calculate_NoClosingParenthesis_ThrowsException()
    {
        ParseException exception = Assert.Throws<ParseException>(() => this.calculator.Calculate<int>("2 low (3 high 4"));
        Assert.Equal((int)ParseExceptionCode.UnparsedToken, exception.Code);
    }

    [Fact]
    public void Calculate_ClosingParenthesisOfDifferentType_ThrowsException()
    {
        ParseException exception = Assert.Throws<ParseException>(() => this.calculator.Calculate<int>("2 low (3 high 4>"));
        Assert.Equal((int)ParseExceptionCode.UnparsedToken, exception.Code);
    }

    [Fact]
    public void Calculate_ListOperand_Correct()
    {
        Token token = this.calculator.Calculate("(1, 2, 3)");

        ListOperand listOperand = Assert.IsType<ListOperand>(token);
        Operand<int> intOperand = Assert.IsType<Operand<int>>(listOperand.Operands[0]);
        Assert.Equal(1, intOperand.Value);

        intOperand = Assert.IsType<Operand<int>>(listOperand.Operands[1]);
        Assert.Equal(2, intOperand.Value);

        intOperand = Assert.IsType<Operand<int>>(listOperand.Operands[2]);
        Assert.Equal(3, intOperand.Value);
    }

    [Theory]
    [InlineData("1 high low 1")]
    [InlineData("1 low high 1")]
    public void Calculate_SeveralOperatorsInRow_ThrowsException(string formula)
    {
        CalculateException exception = Assert.Throws<CalculateException>(() => this.calculator.Calculate<int>(
            formula
        ));
        Assert.Equal((int)CalculateExceptionCode.SubsequentOperators, exception.Code);
    }

    public class CalcLowPriorityOperator : Operator
    {
        public const int ReturnValue = 0;

        private readonly List<Token[]> calls = new();

        public override string Text => "low";

        public IReadOnlyList<Token[]> Calls => this.calls.AsReadOnly();

        public CalcLowPriorityOperator()
            : base(0, 2)
        {
        }

        public override Token Execute(IList<Token> operands)
        {
            this.calls.Add(operands.ToArray());

            return new Operand<int>(ReturnValue);
        }
    }

    public class CalcHighPriorityOperator : Operator
    {
        public const int ReturnValue = 1;

        private readonly List<Token[]> calls = new();

        public override string Text => "high";

        public IReadOnlyList<Token[]> Calls => this.calls.AsReadOnly();

        public CalcHighPriorityOperator()
            : base((OperationPriority)1, 2)
        {
        }

        public override Token Execute(IList<Token> operands)
        {
            this.calls.Add(operands.ToArray());

            return new Operand<int>(ReturnValue);
        }
    }
}