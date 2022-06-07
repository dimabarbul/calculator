using System;
using System.Diagnostics.CodeAnalysis;
using Calculator.Core.Operands;
using Calculator.Core.Parsers;
using Calculator.Core.Tokens;

namespace Calculator.Core.Tests.Utilities;

public class IntParser : IParser
{
    public bool TryParse(ReadOnlySpan<char> formula, [NotNullWhen(true)] out Token? token, out int parsedLength)
    {
        token = null;
        parsedLength = 0;

        int i;

        for (i = 0; i < formula.Length; i++)
        {
            if (!char.IsDigit(formula[i]))
            {
                break;
            }
        }

        if (i > 0)
        {
            int result = int.Parse(formula[..i]);
            token = new Operand<int>(result);
            parsedLength = i;
        }

        return token != null;
    }
}