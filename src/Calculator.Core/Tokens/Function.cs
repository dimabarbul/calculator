using Calculator.Core.Operands;

namespace Calculator.Core.Tokens;

public abstract class Function : Operation
{
    protected Function()
        : base(HighestPriority, 1, isLeftToRight: false)
    {
    }

    public override Token Execute(IReadOnlyList<Token> operands)
    {
        IReadOnlyList<Operand> functionOperands;

        if (operands[0] is ListOperand listOperand)
        {
            functionOperands = listOperand.Operands;
        }
        else if (operands[0] is NullToken)
        {
            functionOperands = Array.Empty<Operand>();
        }
        else
        {
            functionOperands = new[]
            {
                (Operand)operands[0],
            };
        }

        return this.ExecuteFunction(functionOperands);
    }

    public abstract string FunctionName { get; }

    protected abstract Token ExecuteFunction(IReadOnlyList<Operand> operands);
}