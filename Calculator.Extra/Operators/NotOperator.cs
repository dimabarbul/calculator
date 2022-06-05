using Calculator.Core.Enums;

namespace Calculator.Extra.Operators;

public class NotOperator : BoolOperator
{
    public NotOperator()
        : base(OperationPriority.Unary, 1)
    {
    }

    public override string Text => "!";

    protected override bool GetBoolResult(params bool[] operands)
    {
        return !operands[0];
    }
}