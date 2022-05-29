using System.Diagnostics.CodeAnalysis;
using Calculator.Core.Enum;

namespace Calculator.Core.Parser;

public class VariableParser : IParser
{
    public bool TryParse(ReadOnlySpan<char> formula, [NotNullWhen(true)] out Token? token, out int parsedLength)
    {
        token = null;
        parsedLength = default;

        if (formula.IsEmpty || formula[0] != '$')
        {
            return false;
        }

        int index = 1;
        while (index < formula.Length &&
               this.IsValidVariableNameChar(formula[index], isFirstChar: index == 1))
        {
            index++;
        }

        if (index == 1)
        {
            return false;
        }

        token = new Token(formula[1..index].ToString(), TokenType.Variable);
        parsedLength = index;

        return true;
    }

    private bool IsValidVariableNameChar(char c, bool isFirstChar)
    {
        return (!isFirstChar && char.IsDigit(c)) || char.IsLetter(c) || c == '_';
    }
}