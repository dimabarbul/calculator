using Calculator.Core.Extensions;
using Calculator.Core.Operands;

namespace Calculator.Core.Operations.Functions;

public class MinFunction : Function
{
    public override string FunctionName => "min";

    protected override Token ExecuteFunction(params Operand[] operands)
    {
        Operand<decimal>[] decimalOperands = operands.As<decimal>();

        decimal minValue = decimalOperands.Min(o => o.Value);

        return decimalOperands.First(o => o.Value == minValue);
    }
}