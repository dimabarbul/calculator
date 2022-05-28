using Calculator.Core.Enum;

namespace Calculator.Core.Operation;

internal class FloorOperation : DecimalOperationBase
{
    public FloorOperation()
        : base(OperationPriority.Unary, true)
    {
    }

    protected override decimal GetDecimalResult(decimal leftOperand, decimal? rightOperand)
    {
        return Math.Floor(leftOperand);
    }
}