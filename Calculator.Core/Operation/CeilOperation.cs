using Calculator.Core.Enum;

namespace Calculator.Core.Operation;

internal class CeilOperation : DecimalOperationBase
{
    public CeilOperation()
        : base(OperationPriority.Unary, 1)
    {
    }

    protected override decimal GetDecimalResult(params decimal[] operands)
    {
        return Math.Ceiling(operands[0]);
    }
}