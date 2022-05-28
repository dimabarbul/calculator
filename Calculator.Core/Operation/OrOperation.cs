using Calculator.Core.Enum;

namespace Calculator.Core.Operation;

internal class OrOperation : BoolOperationBase
{
    public OrOperation()
        : base(OperationPriority.Or, 2)
    {
    }

    protected override bool GetBoolResult(params bool[] operands)
    {
        return operands[0] || operands[1];
    }
}