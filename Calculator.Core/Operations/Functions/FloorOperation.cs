using Calculator.Core.Extensions;
using Calculator.Core.Operands;

namespace Calculator.Core.Operations.Functions;

internal class FloorOperation : Function
{
    public override string FunctionName => "floor";

    protected override Token ExecuteFunction(params Operand[] operands)
    {
        Operand<decimal>[] decimalOperands = operands.CheckCount(1).As<decimal>();

        return new Operand<decimal>(Math.Floor(decimalOperands[0].Value));
    }
}