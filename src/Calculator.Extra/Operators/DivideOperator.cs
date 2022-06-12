using Calculator.Extra.Enums;

namespace Calculator.Extra.Operators;

public class DivideOperator : BinaryOperator<decimal>
{
    public DivideOperator()
        : base(OperationPriority.Divide)
    {
    }

    public override string Text => "/";

    protected override decimal GetResult(decimal leftOperand, decimal rightOperand)
    {
        return leftOperand / rightOperand;
    }
}