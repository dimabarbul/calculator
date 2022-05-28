using Calculator.Core.Enum;

namespace Calculator.Core.Operation;

internal class AddOperation : DecimalOperationBase
{
    public AddOperation()
        : base(OperationPriority.Add, false)
    {
    }

    protected override decimal GetDecimalResult(decimal leftOperand, decimal? rightOperand)
    {
        return leftOperand + rightOperand.Value;
    }
}