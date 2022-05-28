using Calculator.Core.Enum;

namespace Calculator.Core.Operation;

internal class UnaryPlusOperation : DecimalOperationBase
{
    public UnaryPlusOperation()
        : base(OperationPriority.Unary, 1)
    {
    }

    protected override decimal GetDecimalResult(params decimal[] operands)
    {
        return operands[0];
    }
}