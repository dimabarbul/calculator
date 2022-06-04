using Calculator.Core.Extensions;
using Calculator.Core.Operands;

namespace Calculator.Core.Operations.Functions;

public class MaxFunction : Function
{
    public override string FunctionName => "max";

    protected override Token ExecuteFunction(IReadOnlyList<Operand> operands)
    {
        operands.CheckValueType<decimal>();

        Operand<decimal> result = (Operand<decimal>)operands[0];

        for (int i = 1; i < operands.Count; i++)
        {
            Operand<decimal> operand = ((Operand<decimal>)operands[i]);
            decimal value = operand.Value;
            if (value > result.Value)
            {
                result = operand;
            }
        }

        return result;
    }
}