using Calculator.Core.Enum;

namespace Calculator.Core.Operation;

internal class AddOperation : DecimalOperationBase
{
    public AddOperation()
        : base(OperationPriority.Add, 2)
    {
    }

    protected override decimal GetDecimalResult(params decimal[] operands)
    {
        return operands[0] + operands[1];
    }
}