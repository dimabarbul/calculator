using Calculator.Core.Enums;

namespace Calculator.Extra.Operators;

public class AddOperator : DecimalOperator
{
    public AddOperator()
        : base(OperationPriority.Add, 2, 1)
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