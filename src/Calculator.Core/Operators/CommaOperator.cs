using Calculator.Core.Extensions;
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

    public override Token Execute(IReadOnlyList<Token> operands)
    {
        operands.CheckAllOperands();

        if (operands[0] is ListOperand listOperand)
        {
            listOperand.Add((Operand)operands[1]);
        }
        else
        {
            listOperand = new ListOperand((Operand)operands[0], (Operand)operands[1]);
        }

        return listOperand;
    }
}