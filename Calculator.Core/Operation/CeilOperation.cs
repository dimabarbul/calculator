using Calculator.Core.Enum;

namespace Calculator.Core.Operation;

internal class CeilOperation : DecimalOperationBase
{
    public CeilOperation()
        : base(OperationPriority.Unary, true)
    {
    }

    protected override decimal GetDecimalResult(decimal leftOperand, decimal? rightOperand)
    {
        return Math.Ceiling(leftOperand);
    }
}