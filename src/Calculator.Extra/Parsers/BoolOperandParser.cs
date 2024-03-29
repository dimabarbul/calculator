﻿using System.Diagnostics.CodeAnalysis;
using Calculator.Core.Operands;
using Calculator.Core.Parsers;
using Calculator.Core.ParsingContexts;
using Calculator.Core.Tokens;

namespace Calculator.Extra.Parsers;

public class BoolOperandParser : IParser
{
    private const string TrueString = "true";
    private const string FalseString = "false";

    private static readonly Operand<bool> True = new(true);
    private static readonly Operand<bool> False = new(false);

    public bool TryParse(ReadOnlySpan<char> formula, ParsingContext context, [NotNullWhen(true)] out Token? token,
        out int parsedLength)
    {
        token = null;
        parsedLength = default;

        if (formula.StartsWith(TrueString, StringComparison.Ordinal))
        {
            token = True;
            parsedLength = TrueString.Length;

            return true;
        }

        if (formula.StartsWith(FalseString, StringComparison.Ordinal))
        {
            token = False;
            parsedLength = FalseString.Length;

            return true;
        }

        return false;
    }
}