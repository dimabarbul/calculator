using System.Collections.Generic;
using Calculator.Core.Operands;
using Calculator.Core.ParsingContexts;
using Calculator.Core.Tokens;

namespace Calculator.Core.Tests.ParsingContexts;

public class ParsingContextTest
{
    [Fact]
    public void InitialContext()
    {
        ParsingContext context = ParsingContext.Initial;

        Assert.True(context.IsEndAllowed);
        Assert.False(context.IsBinaryOperatorAllowed);
        Assert.True(context.IsUnaryOperatorAllowed);
        Assert.True(context.IsFunctionAllowed);
        Assert.True(context.IsSubformulaAllowed);
        Assert.True(context.IsVariableAllowed);
        Assert.True(context.IsOperandAllowed);
    }

    [Fact]
    public void AfterOperand()
    {
        ParsingContext context = ParsingContext.Initial.SetNextToken(new ListOperand());

        Assert.True(context.IsEndAllowed);
        Assert.True(context.IsBinaryOperatorAllowed);
        Assert.False(context.IsUnaryOperatorAllowed);
        Assert.False(context.IsFunctionAllowed);
        Assert.False(context.IsSubformulaAllowed);
        Assert.False(context.IsVariableAllowed);
        Assert.False(context.IsOperandAllowed);
    }

    [Fact]
    public void AfterVariable()
    {
        ParsingContext context = ParsingContext.Initial.SetNextToken(new Variable("test"));

        Assert.True(context.IsEndAllowed);
        Assert.True(context.IsBinaryOperatorAllowed);
        Assert.False(context.IsUnaryOperatorAllowed);
        Assert.False(context.IsFunctionAllowed);
        Assert.False(context.IsSubformulaAllowed);
        Assert.False(context.IsVariableAllowed);
        Assert.False(context.IsOperandAllowed);
    }

    [Fact]
    public void AfterBinaryOperator()
    {
        ParsingContext context = ParsingContext.Initial
            .SetNextToken(new ListOperand())
            .SetNextToken(new MyBinaryOperator());

        Assert.False(context.IsEndAllowed);
        Assert.False(context.IsBinaryOperatorAllowed);
        Assert.True(context.IsUnaryOperatorAllowed);
        Assert.True(context.IsFunctionAllowed);
        Assert.True(context.IsSubformulaAllowed);
        Assert.True(context.IsVariableAllowed);
        Assert.True(context.IsOperandAllowed);
    }

    [Fact]
    public void AfterUnaryOperator()
    {
        ParsingContext context = ParsingContext.Initial.SetNextToken(new MyUnaryOperator());

        Assert.False(context.IsEndAllowed);
        Assert.False(context.IsBinaryOperatorAllowed);
        Assert.True(context.IsUnaryOperatorAllowed);
        Assert.True(context.IsFunctionAllowed);
        Assert.True(context.IsSubformulaAllowed);
        Assert.True(context.IsVariableAllowed);
        Assert.True(context.IsOperandAllowed);
    }

    [Fact]
    public void AfterSubformula()
    {
        ParsingContext context = ParsingContext.Initial.SetNextToken(new Subformula(""));

        Assert.True(context.IsEndAllowed);
        Assert.True(context.IsBinaryOperatorAllowed);
        Assert.False(context.IsUnaryOperatorAllowed);
        Assert.False(context.IsFunctionAllowed);
        Assert.False(context.IsSubformulaAllowed);
        Assert.False(context.IsVariableAllowed);
        Assert.False(context.IsOperandAllowed);
    }

    [Fact]
    public void AfterFunction()
    {
        ParsingContext context = ParsingContext.Initial.SetNextToken(new MyFunction());

        Assert.False(context.IsEndAllowed);
        Assert.False(context.IsBinaryOperatorAllowed);
        Assert.False(context.IsUnaryOperatorAllowed);
        Assert.False(context.IsFunctionAllowed);
        Assert.True(context.IsSubformulaAllowed);
        Assert.False(context.IsVariableAllowed);
        Assert.False(context.IsOperandAllowed);
    }

    private class MyBinaryOperator : BinaryOperator
    {
        public override string Text => nameof(ParsingContextTest) + "binary";

        public MyBinaryOperator()
            : base(LowestPriority)
        {
        }

        protected override Token ExecuteBinaryOperator(Operand leftOperand, Operand rightOperand)
        {
            throw new System.NotImplementedException();
        }
    }

    private class MyUnaryOperator : UnaryOperator
    {
        public override string Text => nameof(ParsingContextTest) + "unary";

        protected override Operand ExecuteUnaryOperator(Operand operand)
        {
            throw new System.NotImplementedException();
        }
    }

    private class MyFunction : Function
    {
        public override string FunctionName => nameof(ParsingContextTest) + "func";

        protected override Token ExecuteFunction(IReadOnlyList<Operand> operands)
        {
            throw new System.NotImplementedException();
        }
    }
}