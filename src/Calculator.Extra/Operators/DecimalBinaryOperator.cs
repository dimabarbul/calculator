using Calculator.Core.Operands;
using Calculator.Core.Tokens;
using Calculator.Extra.Enums;

namespace Calculator.Extra.Operators;

public abstract class DecimalBinaryOperator : BinaryOperator
{
    protected DecimalBinaryOperator(OperationPriority priority)
        : base((int)(LowestPriority + priority))
    {
    }

    public override Token Execute(IList<Token> operands)
    {
        this.ValidateOperandsCount(operands);

        decimal[] decimalValues = new decimal[operands.Count];
        for (int i = 0; i < operands.Count; i++)
        {
            if (operands[i] is not Operand<decimal> decimalOperand)
            {
                throw new ArgumentException("Decimal operation can be performed only on decimal operands", nameof(operands));
            }

            decimalValues[i] = decimalOperand.Value;
        }


        decimal result = this.GetDecimalResult(decimalValues);

        return new Operand<decimal>(result);
    }

    protected abstract decimal GetDecimalResult(params decimal[] operands);
}