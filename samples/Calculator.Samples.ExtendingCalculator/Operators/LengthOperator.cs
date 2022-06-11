using Calculator.Core.Operands;
using Calculator.Core.Tokens;

namespace Calculator.Samples.ExtendingCalculator.Operators;

public class LengthOperator : UnaryOperator
{
    public override string Text => "||";

    protected override Operand ExecuteUnaryOperator(Operand operand)
    {
        return operand switch
        {
            ListOperand listOperand => this.GetLengthOfList(listOperand),
            Operand<string> stringOperand => this.GetLengthOfString(stringOperand),
            _ => throw new ArgumentException("Length operator can be performed only on list or string"),
        };
    }

    private Operand GetLengthOfList(ListOperand listOperand)
    {
        decimal sqrSum = 0;
        foreach (Operand listOperandOperand in listOperand.Operands)
        {
            if (listOperandOperand is not Operand<decimal> decimalOperand)
            {
                throw new ArgumentException($"Operator {this.Text} can only be performed on list of decimals");
            }

            sqrSum += decimalOperand.Value * decimalOperand.Value;
        }

        return new Operand<decimal>((decimal)Math.Sqrt((double)sqrSum));
    }

    private Operand GetLengthOfString(Operand<string> stringOperand)
    {
        return new Operand<decimal>(stringOperand.Value.Length);
    }
}