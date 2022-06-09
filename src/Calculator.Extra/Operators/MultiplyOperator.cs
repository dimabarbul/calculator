using Calculator.Extra.Enums;

namespace Calculator.Extra.Operators;

public class MultiplyOperator : DecimalBinaryOperator
{
    public MultiplyOperator()
        : base(OperationPriority.Multiply)
    {
    }

    public override string Text => "*";

    protected override decimal GetDecimalResult(params decimal[] operands)
    {
        return operands[0] * operands[1];
    }
}