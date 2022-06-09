using Calculator.Core.Tokens;

namespace Calculator.Core.Operands;

public class Operand<TValue> : Operand
{
    public Operand(TValue value)
    {
        this.Value = value;
    }

    public TValue Value { get; }
}