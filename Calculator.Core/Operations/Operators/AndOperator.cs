using Calculator.Core.Enums;

namespace Calculator.Core.Operations.Operators;

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