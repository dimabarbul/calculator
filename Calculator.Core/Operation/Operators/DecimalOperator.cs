using Calculator.Core.Enum;
using Calculator.Core.Operands;

namespace Calculator.Core.Operation.Operators;

internal abstract class DecimalOperator : Operator
{
    protected DecimalOperator(OperationPriority priority, int operandsCount, int? minOperandsCount = null)
        : base(priority, operandsCount, minOperandsCount)
    {
    }

    public override Token Execute(params Token[] operands)
    {
        this.ValidateOperandsCount(operands);

        if (operands.Any(o => o is not Operand<decimal>))
        {
            throw new ArgumentException("Decimal operation can be performed only on decimal operands", nameof(operands));
        }

        decimal result = this.GetDecimalResult(
            operands.Cast<Operand<decimal>>().Select(o => o.Value).ToArray()
        );

        return new Operand<decimal>(result);
    }

    protected abstract decimal GetDecimalResult(params decimal[] operands);
}