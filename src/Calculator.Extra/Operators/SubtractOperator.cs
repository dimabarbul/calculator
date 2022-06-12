using Calculator.Extra.Enums;

namespace Calculator.Extra.Operators;

public class SubtractOperator : BinaryOperator<decimal>
{
    public SubtractOperator()
        : base(OperationPriority.Subtract)
    {
    }

    public override string Text => "-";

    protected override decimal GetResult(decimal leftOperand, decimal rightOperand)
    {
        return leftOperand - rightOperand;
    }
}