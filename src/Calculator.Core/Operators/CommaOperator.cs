using Calculator.Core.Operands;
using Calculator.Core.Tokens;

namespace Calculator.Core.Operators;

public class CommaOperator : BinaryOperator
{
    public CommaOperator()
        : base(LowestPriority)
    {
    }

    public override string Text => ",";

    protected override Token ExecuteBinaryOperator(Operand leftOperand, Operand rightOperand)
    {
        if (leftOperand is ListOperand listOperand)
        {
            listOperand.Add(rightOperand);
        }
        else
        {
            listOperand = new ListOperand(leftOperand, rightOperand);
        }

        return listOperand;
    }
}