using Calculator.Core.Enum;

namespace Calculator.Core.Operation;

internal class UnaryPlusOperation : DecimalOperationBase
{
    public UnaryPlusOperation()
        : base(OperationPriority.Unary, true)
    {
    }

    protected override decimal GetDecimalResult(decimal leftOperand, decimal? rightOperand)
    {
        return leftOperand;
    }
}