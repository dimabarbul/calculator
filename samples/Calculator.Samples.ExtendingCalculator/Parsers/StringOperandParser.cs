using System.Diagnostics.CodeAnalysis;
using Calculator.Core.Operands;
using Calculator.Core.Parsers;
using Calculator.Core.ParsingContexts;
using Calculator.Core.Tokens;

namespace Calculator.Samples.ExtendingCalculator.Parsers;

public class StringOperandParser : IParser
{
    public bool TryParse(
        ReadOnlySpan<char> formula,
        ParsingContext context,
        [NotNullWhen(true)] out Token? token,
        out int parsedLength)
    {
        token = null;
        parsedLength = default;

        if (formula[0] != '"')
        {
            return false;
        }

        bool isEscaped = false;
        int i;
        for (i = 1; i < formula.Length; i++)
        {
            if (formula[i] == '\\' && !isEscaped)
            {
                isEscaped = true;
            }
            else if (formula[i] == '"' && !isEscaped)
            {
                break;
            }
            else
            {
                isEscaped = false;
            }
        }

        if (i == formula.Length)
        {
            return false;
        }

        token = new Operand<string>(
            formula[1..i]
                .ToString()
                .Replace("\\\"", "\"")
                .Replace("\\\\", "\\"));
        parsedLength = i + 1;

        return true;
    }
}