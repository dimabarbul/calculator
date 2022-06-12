using Calculator.Core.Operands;
using Calculator.Core.Tokens;
using Calculator.Extra.Enums;

namespace Calculator.Extra.Operators;

public abstract class BinaryOperator<TValue> : BinaryOperator
{
    protected BinaryOperator(OperationPriority priority)
        : base((int)(LowestPriority + priority))
    {
    }

    protected override Token ExecuteBinaryOperator(Operand leftOperand, Operand rightOperand)
    {
        if (leftOperand is not Operand<TValue> leftTypedOperand
            || rightOperand is not Operand<TValue> rightTypedOperand)
        {
            throw new ArgumentException("Operation can be performed only on TValue operands");
        }

        TValue result = this.GetResult(leftTypedOperand.Value, rightTypedOperand.Value);

        return new Operand<TValue>(result);
    }

    protected abstract TValue GetResult(TValue leftOperand, TValue rightOperand);
}