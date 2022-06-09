using Calculator.Extra.Enums;

namespace Calculator.Extra.Operators;

public class AddOperator : DecimalBinaryOperator
{
    public AddOperator()
        : base(OperationPriority.Add)
    {
    }

    public override string Text => "+";

    protected override decimal GetDecimalResult(params decimal[] operands)
    {
        if (operands.Length == 1)
        {
            return operands[0];
        }

        return operands[0] + operands[1];
    }
}