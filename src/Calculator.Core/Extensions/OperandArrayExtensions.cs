using Calculator.Core.Operands;
using Calculator.Core.Tokens;

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

    public static void CheckValueType<TValue>(this IEnumerable<Token> tokens)
    {
        foreach (Token token in tokens)
        {
            if (token is not Operand<TValue>)
            {
                throw new ArgumentException($"Expected all operands to be of type {typeof(Operand<TValue>)}");
            }
        }
    }

    public static void CheckAllOperands(this IEnumerable<Token> tokens)
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