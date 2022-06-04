using Calculator.Core.Extensions;
using Calculator.Core.Operands;

namespace Calculator.Core.Operations.Functions;

internal class FloorOperation : Function
{
    public override string FunctionName => "floor";

    protected override Token ExecuteFunction(IReadOnlyList<Operand> operands)
    {
        operands
            .CheckCount(1)
            .CheckValueType<decimal>();
        Operand<decimal> decimalOperand = (Operand<decimal>)operands[0];

        return new Operand<decimal>(Math.Floor(decimalOperand.Value));
    }
}