using Calculator.Core.Enum;

namespace Calculator.Core.Operation.Operators;

internal class AndOperator : BoolOperator
{
    public AndOperator()
        : base(OperationPriority.And, 2)
    {
    }

    public override string Text => "&&";

    protected override bool GetBoolResult(params bool[] operands)
    {
        return operands[0] && operands[1];
    }
}