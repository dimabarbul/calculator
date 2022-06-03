using Calculator.Core.Operands;

namespace Calculator.Core.Extensions;

public static class OperandArrayExtensions
{
    public static Operand[] CheckCount(this Operand[] operands, int count)
    {
        if (operands.Length != count)
        {
            throw new ArgumentException($"Expected {count} operands, got {operands.Length}");
        }

        return operands;
    }

    public static Operand<TValue>[] As<TValue>(this Operand[] operands)
    {
        if (operands.Any(o => o is not Operand<TValue>))
        {
            throw new ArgumentException($"Expected all operands to be of type {typeof(Operand<TValue>)}");
        }

        return operands
            .Cast<Operand<TValue>>()
            .ToArray();
    }

    public static TValue[] GetValues<TValue>(this Operand[] operands)
    {
        return operands
            .As<TValue>()
            .Select(o => o.Value)
            .ToArray();
    }
}