using Calculator.Core.Enum;

namespace Calculator.Core.Operation;

internal class UnaryMinusOperation : DecimalOperationBase
{
    public UnaryMinusOperation()
        : base(OperationPriority.Unary, true)
    {
    }

    protected override decimal GetDecimalResult(decimal leftOperand, decimal? rightOperand)
    {
        return -leftOperand;
    }
}