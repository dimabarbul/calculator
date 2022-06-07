using Calculator.Core.Extensions;
using Calculator.Core.Operands;
using Calculator.Core.Tokens;

namespace Calculator.Core.Operators;

public class CommaOperator : Operator
{
    public CommaOperator()
        : base(LowestPriority, 2)
    {
    }

    public override string Text => ",";

    public override Token Execute(IList<Token> operands)
    {
        operands.CheckAllOperands();

        if (operands[0] is ListOperand listOperand)
        {
            listOperand.Add((Operand)operands[1]);
        }
        else
        {
            listOperand = new((Operand)operands[0], (Operand)operands[1]);
        }

        return listOperand;
    }
}