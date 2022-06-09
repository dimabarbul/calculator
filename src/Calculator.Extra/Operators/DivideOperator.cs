using Calculator.Extra.Enums;

namespace Calculator.Extra.Operators;

public class DivideOperator : DecimalBinaryOperator
{
    public DivideOperator()
        : base(OperationPriority.Divide)
    {
    }

    public override string Text => "/";

    protected override decimal GetDecimalResult(params decimal[] operands)
    {
        return operands[0] / operands[1];
    }
}