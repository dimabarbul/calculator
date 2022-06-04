using Calculator.Core.Enums;
using Calculator.Core.Operands;

namespace Calculator.Core.Operations.Operators;

internal abstract class BoolOperator : Operator
{
    protected BoolOperator(OperationPriority priority, int operandsCount, int? minOperandsCount = null)
        : base(priority, operandsCount, minOperandsCount)
    {
    }

    public override Token Execute(IList<Token> operands)
    {
        this.ValidateOperandsCount(operands);

        bool[] boolValues = new bool[operands.Count];
        for (int i = 0; i < operands.Count; i++)
        {
            if (operands[i] is not Operand<bool> boolOperand)
            {
                throw new ArgumentException("Decimal operation can be performed only on decimal operands", nameof(operands));
            }

            boolValues[i] = boolOperand.Value;
        }

        bool result = this.GetBoolResult(boolValues);

        return new Operand<bool>(result);
    }

    protected abstract bool GetBoolResult(params bool[] operands);
}