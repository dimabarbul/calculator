using Calculator.Core.Enum;

namespace Calculator.Core.Operation;

internal class OrOperation : BoolOperationBase
{
    public OrOperation()
        : base(OperationPriority.Or, false)
    {
    }

    protected override bool GetBoolResult(bool leftOperand, bool? rightOperand)
    {
        return leftOperand || rightOperand.Value;
    }
}