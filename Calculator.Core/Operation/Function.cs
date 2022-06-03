using Calculator.Core.Enum;
using Calculator.Core.Operands;

namespace Calculator.Core.Operation;

public abstract class Function : Operation
{
    protected Function()
        : base(OperationPriority.Unary, 1)
    {
    }

    public override Token Execute(params Token[] operands)
    {
        Operand[] functionOperands;

        if (operands[0] is ListOperand listOperand)
        {
            functionOperands = listOperand.Operands.ToArray();
        }
        else
        {
            functionOperands = operands.Cast<Operand>().ToArray();
        }

        return this.ExecuteFunction(functionOperands);
    }

    public abstract string FunctionName { get; }

    protected abstract Token ExecuteFunction(params Operand[] operands);
}