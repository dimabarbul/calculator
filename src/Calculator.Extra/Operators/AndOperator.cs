using Calculator.Extra.Enums;

namespace Calculator.Extra.Operators;

public class AndOperator : BoolBinaryOperator
{
    public AndOperator()
        : base(OperationPriority.And)
    {
    }

    public override string Text => "&&";

    protected override bool GetBoolResult(params bool[] operands)
    {
        return operands[0] && operands[1];
    }
}