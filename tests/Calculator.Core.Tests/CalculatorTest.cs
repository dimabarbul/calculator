using System;
using System.Collections.Generic;
using System.Linq;
using Calculator.Core.Enums;
using Calculator.Core.Exceptions;
using Calculator.Core.Operands;
using Calculator.Core.Tests.Extensions;
using Calculator.Core.Tokens;

namespace Calculator.Core.Tests;

public class CalculatorTest
{
    private readonly Calculator calculator;
    private readonly LowPriorityOperator lowPriorityOperator;
    private readonly HighPriorityOperator highPriorityOperator;
    private readonly MyUnaryOperator myUnaryOperator;
    private readonly MyFunction myFunction;

    public CalculatorTest(Calculator calculator, IEnumerable<Operation> operations)
    {
        this.calculator = calculator;

        Operation[] operationsArray = operations.ToArray();

        this.lowPriorityOperator = (LowPriorityOperator)operationsArray.First(o => o is LowPriorityOperator);
        this.highPriorityOperator = (HighPriorityOperator)operationsArray.First(o => o is HighPriorityOperator);
        this.myUnaryOperator = (MyUnaryOperator)operationsArray.First(o => o is MyUnaryOperator);
        this.myFunction = (MyFunction)operationsArray.First(o => o is MyFunction);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Calculate_EmptyFormulaWithTypedResult_ThrowsException(string formula)
    {
        Assert.Throws<ArgumentNullException>(() => this.calculator.Calculate<int>(formula));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Calculate_EmptyFormulaWithTokenResult_ReturnsNullToken(string formula)
    {
        Token result = this.calculator.Calculate(formula);

        Assert.Equal(NullToken.Instance, result);
    }

    [Theory]
    [InlineData("1 low 2", LowPriorityOperator.ReturnValue)]
    [InlineData("1 high 2", HighPriorityOperator.ReturnValue)]
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
            o => Assert.Equal(HighPriorityOperator.ReturnValue, o.Value));

        Assert.Equal(LowPriorityOperator.ReturnValue, result);
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
            o => Assert.Equal(LowPriorityOperator.ReturnValue, o.Value));
        AssertExtensions.TokenIs<Operand<int>>(
            this.highPriorityOperator.Calls[0][1],
            o => Assert.Equal(3, o.Value));

        Assert.Equal(HighPriorityOperator.ReturnValue, result);
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
        ParseException exception = Assert.Throws<ParseException>(() => this.calculator.Calculate<int>("1 2"));
        Assert.Equal((int)ParseExceptionCode.MisplacedToken, exception.Code);
    }

    [Fact]
    public void Calculate_BinaryOperationMissingLeftOperand_ThrowsException()
    {
        ParseException exception = Assert.Throws<ParseException>(() => this.calculator.Calculate<int>("low 2"));
        Assert.Equal((int)ParseExceptionCode.UnparsedToken, exception.Code);
    }

    [Fact]
    public void Calculate_BinaryOperationMissingBothOperands_ThrowsException()
    {
        ParseException exception = Assert.Throws<ParseException>(() => this.calculator.Calculate<int>("high"));
        Assert.Equal((int)ParseExceptionCode.UnparsedToken, exception.Code);
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
        ParseException exception = Assert.Throws<ParseException>(() => this.calculator.Calculate<int>(
            formula
        ));
        Assert.Equal((int)ParseExceptionCode.UnparsedToken, exception.Code);
    }

    [Theory]
    [InlineData("unary 1", 1)]
    [InlineData("unary func(1)", 1)]
    [InlineData("unary unary unary 1", 3)]
    [InlineData("1 high unary 1", 1)]
    [InlineData("1 high (unary 1)", 1)]
    [InlineData("(unary 1) low 1", 1)]
    public void Calculate_UnaryOperatorInCorrectContext_Calculated(string formula, int unaryOperatorCallsCount)
    {
        Exception? exception = Record.Exception(() => this.calculator.Calculate(formula));

        Assert.Null(exception);
        Assert.Equal(unaryOperatorCallsCount, this.myUnaryOperator.Calls.Count);
    }

    [Theory]
    [InlineData("1 unary 1")]
    [InlineData("func(1) unary 1")]
    [InlineData("1 unary high 1")]
    [InlineData("1 high (1 unary 1)")]
    [InlineData("unary")]
    public void Calculate_UnaryOperatorInWrongContext_ThrowsException(string formula)
    {
        Assert.Throws<ParseException>(() => this.calculator.Calculate(formula));
    }

    [Theory]
    [InlineData("func")]
    public void Calculate_FunctionInWrongContext_ThrowsException(string formula)
    {
        ParseException exception = Assert.Throws<ParseException>(() => this.calculator.Calculate(formula));

        Assert.Equal((int)ParseExceptionCode.UnexpectedEnd, exception.Code);
    }

    [Fact]
    public void Calculate_FunctionWithoutArguments_Calculated()
    {
        int result = this.calculator.Calculate<int>("func()");

        Assert.Equal(MyFunction.ReturnValue, result);
        Assert.Equal(1, this.myFunction.Calls.Count);
        Assert.Equal(Array.Empty<Operand>(), this.myFunction.Calls[0]);
    }

    public class LowPriorityOperator : BinaryOperator
    {
        public const int ReturnValue = 0;

        private readonly List<Token[]> calls = new();

        public override string Text => "low";

        public IReadOnlyList<Token[]> Calls => this.calls.AsReadOnly();

        public LowPriorityOperator()
            : base(0)
        {
        }

        public override Token Execute(IReadOnlyList<Token> operands)
        {
            this.calls.Add(operands.ToArray());

            return new Operand<int>(ReturnValue);
        }
    }

    public class HighPriorityOperator : BinaryOperator
    {
        public const int ReturnValue = 1;

        private readonly List<Token[]> calls = new();

        public override string Text => "high";

        public IReadOnlyList<Token[]> Calls => this.calls.AsReadOnly();

        public HighPriorityOperator()
            : base(1)
        {
        }

        public override Token Execute(IReadOnlyList<Token> operands)
        {
            this.calls.Add(operands.ToArray());

            return new Operand<int>(ReturnValue);
        }
    }

    public class MyUnaryOperator : UnaryOperator
    {
        public const int ReturnValue = 2;

        private readonly List<Token> calls = new();

        public override string Text => "unary";

        protected override Operand ExecuteUnaryOperator(Operand operand)
        {
            this.calls.Add(operand);

            return new Operand<int>(ReturnValue);
        }

        public IReadOnlyList<Token> Calls => this.calls.AsReadOnly();
    }

    public class MyFunction : Function
    {
        public const int ReturnValue = 3;

        private readonly List<Operand[]> calls = new();

        public override string FunctionName => "func";

        public IReadOnlyList<Operand[]> Calls => this.calls.AsReadOnly();

        protected override Token ExecuteFunction(IReadOnlyList<Operand> operands)
        {
            this.calls.Add(operands.ToArray());

            return new Operand<int>(ReturnValue);
        }
    }
}