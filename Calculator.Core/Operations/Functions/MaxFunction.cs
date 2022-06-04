using Calculator.Core.Extensions;
using Calculator.Core.Operands;

namespace Calculator.Core.Operations.Functions;

public class MaxFunction : Function
{
    public override string FunctionName => "max";

    protected override Token ExecuteFunction(params Operand[] operands)
    {
        Operand<decimal>[] decimalOperands = operands.As<decimal>();

        decimal maxValue = decimalOperands.Max(o => o.Value);

        return decimalOperands.First(o => o.Value == maxValue);
    }
}