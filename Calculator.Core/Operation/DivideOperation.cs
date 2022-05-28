using Calculator.Core.Enum;

namespace Calculator.Core.Operation;

internal class DivideOperation : DecimalOperationBase
{
    public DivideOperation()
        : base(OperationPriority.Divide, 2)
    {
    }

    protected override decimal GetDecimalResult(params decimal[] operands)
    {
        return operands[0] / operands[1];
    }
}