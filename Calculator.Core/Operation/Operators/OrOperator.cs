using Calculator.Core.Enum;

namespace Calculator.Core.Operation.Operators;

internal class OrOperator : BoolOperator
{
    public OrOperator()
        : base(OperationPriority.Or, 2)
    {
    }

    public override string Text => "||";

    protected override bool GetBoolResult(params bool[] operands)
    {
        return operands[0] || operands[1];
    }
}