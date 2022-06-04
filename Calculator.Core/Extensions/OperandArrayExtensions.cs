using Calculator.Core.Operands;

namespace Calculator.Core.Extensions;

public static class OperandArrayExtensions
{
    public static IReadOnlyList<Operand> CheckCount(this IReadOnlyList<Operand> operands, int count)
    {
        if (operands.Count != count)
        {
            throw new ArgumentException($"Expected {count} operands, got {operands.Count}");
        }

        return operands;
    }

    public static void CheckValueType<TValue>(this IReadOnlyList<Operand> operands)
    {
        foreach (Operand operand in operands)
        {
            if (operand is not Operand<TValue>)
            {
                throw new ArgumentException($"Expected all operands to be of type {typeof(Operand<TValue>)}");
            }
        }
    }

    public static void CheckAllOperands(this IList<Token> tokens)
    {
        foreach (Token token in tokens)
        {
            if (token is not Operand)
            {
                throw new ArgumentException($"Expected all operands to be of type {typeof(Operand)}");
            }
        }
    }
}