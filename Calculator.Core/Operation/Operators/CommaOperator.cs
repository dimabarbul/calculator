using Calculator.Core.Enum;
using Calculator.Core.Operands;

namespace Calculator.Core.Operation.Operators;

public class CommaOperator : Operator
{
    public CommaOperator()
        : base(OperationPriority.Comma, 2)
    {
    }

    public override string Text => ",";

    public override Token Execute(params Token[] operands)
    {
        if (operands.Any(o => o is not Operand))
        {
            throw new ArgumentException("Comma operator can operate only operands.", nameof(operands));
        }

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