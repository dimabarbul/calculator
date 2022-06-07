using Calculator.Extra.Enums;

namespace Calculator.Extra.Operators;

public class MultiplyOperator : DecimalOperator
{
    public MultiplyOperator()
        : base(OperationPriority.Multiply, 2)
    {
    }

    public override string Text => "*";

    protected override decimal GetDecimalResult(params decimal[] operands)
    {
        return operands[0] * operands[1];
    }
}