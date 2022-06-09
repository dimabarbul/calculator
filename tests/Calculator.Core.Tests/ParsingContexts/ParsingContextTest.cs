using Calculator.Core.ParsingContexts;

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
        ParsingContext context = new AfterOperandContext();

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
        ParsingContext context = new AfterVariableContext();

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
        ParsingContext context = new AfterBinaryOperatorContext();

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
        ParsingContext context = new AfterUnaryOperatorContext();

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
        ParsingContext context = new AfterSubformulaContext();

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
        ParsingContext context = new AfterFunctionContext();

        Assert.False(context.IsEndAllowed);
        Assert.False(context.IsBinaryOperatorAllowed);
        Assert.False(context.IsUnaryOperatorAllowed);
        Assert.False(context.IsFunctionAllowed);
        Assert.True(context.IsSubformulaAllowed);
        Assert.False(context.IsVariableAllowed);
        Assert.False(context.IsOperandAllowed);
    }
}