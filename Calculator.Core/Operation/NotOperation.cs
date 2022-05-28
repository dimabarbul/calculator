using Calculator.Core.Enum;

namespace Calculator.Core.Operation;

internal class NotOperation : BoolOperationBase
{
    public NotOperation()
        : base(OperationPriority.Unary, 1)
    {
    }

    protected override bool GetBoolResult(params bool[] operands)
    {
        return !operands[0];
    }
}