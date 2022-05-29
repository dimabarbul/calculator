using System.Diagnostics.CodeAnalysis;
using Calculator.Core.Enum;

namespace Calculator.Core.Parser;

public class FunctionParser : IParser
{
    public bool TryParse(ReadOnlySpan<char> formula, [NotNullWhen(true)] out Token? token, out int parsedLength)
    {
        token = null;
        parsedLength = default;

        if (formula.IsEmpty)
        {
            return false;
        }

        int index = 0;
        while (index < formula.Length &&
            this.IsValidFunctionNameChar(formula[index], isFirstChar: index == 0))
        {
            index++;
        }

        if (index == 0)
        {
            return false;
        }

        if (index >= formula.Length - 1 ||
            !this.IsOpeningBracket(formula[index]))
        {
            return false;
        }

        token = new Token(formula[..index].ToString(), TokenType.Operation);
        parsedLength = index;

        return true;
    }

    private bool IsValidFunctionNameChar(char c, bool isFirstChar)
    {
        return (!isFirstChar && char.IsDigit(c)) || char.IsLetter(c) || c == '_';
    }

    private bool IsOpeningBracket(char c)
    {
        return c is '{' or '[' or '(' or '<';
    }
}