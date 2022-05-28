using Calculator.Core.Enum;

namespace Calculator.Core.Operation;

internal class UnaryMinusOperation : DecimalOperationBase
{
    public UnaryMinusOperation()
        : base(OperationPriority.Unary, 1)
    {
    }

    protected override decimal GetDecimalResult(params decimal[] operands)
    {
        return -operands[0];
    }
}