using Calculator.Core.Enum;

namespace Calculator.Core.Operation;

internal class AndOperation : BoolOperationBase
{
    public AndOperation()
        : base(OperationPriority.And, 2)
    {
    }

    protected override bool GetBoolResult(params bool[] operands)
    {
        return operands[0] && operands[1];
    }
}