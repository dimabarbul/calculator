using Calculator.Extra.Enums;

namespace Calculator.Extra.Operators;

public class OrOperator : BinaryOperator<bool>
{
    public OrOperator()
        : base(OperationPriority.Or)
    {
    }

    public override string Text => "||";

    protected override bool GetResult(bool leftOperand, bool rightOperand)
    {
        return leftOperand || rightOperand;
    }
}