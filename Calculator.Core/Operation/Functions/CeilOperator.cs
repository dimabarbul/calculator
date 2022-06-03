using Calculator.Core.Extensions;
using Calculator.Core.Operands;

namespace Calculator.Core.Operation.Functions;

internal class CeilOperator : Function
{
    public override string FunctionName => "ceil";

    protected override Token ExecuteFunction(params Operand[] operands)
    {
        Operand<decimal>[] decimalOperands = operands.CheckCount(1).As<decimal>();

        return new Operand<decimal>(Math.Ceiling(decimalOperands[0].Value));
    }
}