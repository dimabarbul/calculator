using Calculator.Extra.Enums;

namespace Calculator.Extra.Operators;

public class AndOperator : BinaryOperator<bool>
{
    public AndOperator()
        : base(OperationPriority.And)
    {
    }

    public override string Text => "&&";

    protected override bool GetResult(bool leftOperand, bool rightOperand)
    {
        return leftOperand && rightOperand;
    }
}