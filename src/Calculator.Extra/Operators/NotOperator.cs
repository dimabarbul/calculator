using Calculator.Core.Operands;
using Calculator.Core.Tokens;

namespace Calculator.Extra.Operators;

public class NotOperator : UnaryOperator
{
    public override string Text => "!";

    protected override Operand ExecuteUnaryOperator(Operand operand)
    {
        if (operand is not Operand<bool> boolOperand)
        {
            throw new ArgumentException($"Operator {this.Text} can be performed only on boolean operand");
        }

        return new Operand<bool>(!boolOperand.Value);
    }
}