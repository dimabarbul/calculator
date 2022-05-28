using Calculator.Core.Enum;

namespace Calculator.Core.Operation;

internal class SubtractOperation : DecimalOperationBase
{
    public SubtractOperation()
        : base(OperationPriority.Subtract, 2)
    {
    }

    protected override decimal GetDecimalResult(params decimal[] operands)
    {
        return operands[0] - operands[1];
    }
}