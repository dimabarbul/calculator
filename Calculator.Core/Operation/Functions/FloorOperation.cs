using Calculator.Core.Enum;
using Calculator.Core.Operands;

namespace Calculator.Core.Operation.Functions;

internal class FloorOperation : Function
{
    public FloorOperation()
        : base(OperationPriority.Unary, 1)
    {
    }

    public override string FunctionName => "floor";

    public override Token Execute(params Token[] operands)
    {
        if (operands[0] is not Operand<decimal> decimalOperand)
        {
            throw new ArgumentException("floor can only be applied to decimal operand", nameof(operands));
        }

        return new Operand<decimal>(Math.Floor(decimalOperand.Value));
    }
}