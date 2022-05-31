using Calculator.Core.Enum;
using Calculator.Core.Operands;

namespace Calculator.Core.Operation.Operators;

internal abstract class BoolOperator : Operator
{
    protected BoolOperator(OperationPriority priority, int operandsCount, int? minOperandsCount = null)
        : base(priority, operandsCount, minOperandsCount)
    {
    }

    public override Token Execute(params Token[] operands)
    {
        this.ValidateOperandsCount(operands);

        if (operands.Any(o => o is not Operand<bool>))
        {
            throw new ArgumentException("Decimal operation can be performed only on decimal operands", nameof(operands));
        }

        bool result = this.GetBoolResult(
            operands.Cast<Operand<bool>>().Select(o => o.Value).ToArray()
        );

        return new Operand<bool>(result);
    }

    protected abstract bool GetBoolResult(params bool[] operands);
}