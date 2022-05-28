using Calculator.Core.Enum;

namespace Calculator.Core.Operation;

internal class AndOperation : BoolOperationBase
{
    public AndOperation()
        : base(OperationPriority.And, false)
    {
    }

    protected override bool GetBoolResult(bool leftOperand, bool? rightOperand)
    {
        return leftOperand && rightOperand.Value;
    }
}