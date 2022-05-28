using Calculator.Core.Enum;

namespace Calculator.Core.Operation;

internal class FloorOperation : DecimalOperationBase
{
    public FloorOperation()
        : base(OperationPriority.Unary, 1)
    {
    }

    protected override decimal GetDecimalResult(params decimal[] operands)
    {
        return Math.Floor(operands[0]);
    }
}