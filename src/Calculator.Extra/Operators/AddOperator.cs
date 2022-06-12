using Calculator.Extra.Enums;

namespace Calculator.Extra.Operators;

public class AddOperator : BinaryOperator<decimal>
{
    public AddOperator()
        : base(OperationPriority.Add)
    {
    }

    public override string Text => "+";

    protected override decimal GetResult(decimal leftOperand, decimal rightOperand)
    {
        return leftOperand + rightOperand;
    }
}