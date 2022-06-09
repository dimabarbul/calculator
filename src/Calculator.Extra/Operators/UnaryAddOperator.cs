using Calculator.Core.Operands;
using Calculator.Core.Tokens;

namespace Calculator.Extra.Operators;

public class UnaryAddOperator : UnaryOperator
{
    public override string Text => "+";

    protected override Operand ExecuteUnaryOperator(Operand operand)
    {
        if (operand is not Operand<decimal> decimalOperand)
        {
            throw new ArgumentException($"Operator {this.Text} can only be performed on decimal value.");
        }

        return decimalOperand;
    }
}