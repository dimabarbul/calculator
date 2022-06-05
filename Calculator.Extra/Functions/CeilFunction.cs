using Calculator.Core.Extensions;
using Calculator.Core.Operands;
using Calculator.Core.Tokens;

namespace Calculator.Extra.Functions;

public class CeilFunction : Function
{
    public override string FunctionName => "ceil";

    protected override Token ExecuteFunction(IReadOnlyList<Operand> operands)
    {
        operands
            .CheckCount(1)
            .CheckValueType<decimal>();
        Operand<decimal> decimalOperand = (Operand<decimal>)operands[0];

        return new Operand<decimal>(Math.Ceiling(decimalOperand.Value));
    }
}