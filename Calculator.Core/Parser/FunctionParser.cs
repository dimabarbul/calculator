﻿using System.Diagnostics.CodeAnalysis;
using Calculator.Core.Operation;

namespace Calculator.Core.Parser;

public class FunctionParser : IParser
{
    private readonly Dictionary<string, Function> operations;

    public FunctionParser(IEnumerable<Operation.Operation> operations)
    {
        this.operations = operations
            .Where(o => o is Function)
            .Cast<Function>()
            .ToDictionary(o => o.FunctionName);
    }

    public bool TryParse(ReadOnlySpan<char> formula, [NotNullWhen(true)] out Token? token, out int parsedLength)
    {
        token = null;
        parsedLength = default;

        Token? foundFunction = null;

        if (formula.IsEmpty)
        {
            return false;
        }

        foreach ((string text, Function function) in this.operations)
        {
            if (formula.StartsWith(text, StringComparison.Ordinal))
            {
                foundFunction = function;
                parsedLength = text.Length;
            }
        }

        if (foundFunction == null)
        {
            return false;
        }

        if (parsedLength == formula.Length ||
            !this.IsOpeningBracket(formula[parsedLength]))
        {
            return false;
        }

        token = foundFunction;

        return true;
    }

    private bool IsOpeningBracket(char c)
    {
        return c is '{' or '[' or '(' or '<';
    }
}