using Calculator.Extra.Enums;

namespace Calculator.Extra.Operators;

public class OrOperator : BoolBinaryOperator
{
    public OrOperator()
        : base(OperationPriority.Or)
    {
    }

    public override string Text => "||";

    protected override bool GetBoolResult(params bool[] operands)
    {
        return operands[0] || operands[1];
    }
}