using Calculator.Extra.Enums;

namespace Calculator.Extra.Operators;

public class MultiplyOperator : BinaryOperator<decimal>
{
    public MultiplyOperator()
        : base(OperationPriority.Multiply)
    {
    }

    public override string Text => "*";

    protected override decimal GetResult(decimal leftOperand, decimal rightOperand)
    {
        return leftOperand * rightOperand;
    }
}