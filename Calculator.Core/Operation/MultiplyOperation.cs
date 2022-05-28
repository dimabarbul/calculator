using Calculator.Core.Enum;

namespace Calculator.Core.Operation;

internal class MultiplyOperation : DecimalOperationBase
{
    public MultiplyOperation()
        : base(OperationPriority.Multiply, 2)
    {
    }

    protected override decimal GetDecimalResult(params decimal[] operands)
    {
        return operands[0] * operands[1];
    }
}