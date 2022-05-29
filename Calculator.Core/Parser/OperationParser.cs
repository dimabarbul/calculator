using System.Diagnostics.CodeAnalysis;
using Calculator.Core.Enum;

namespace Calculator.Core.Parser;

public class OperationParser : IParser
{
    public bool TryParse(ReadOnlySpan<char> formula, [NotNullWhen(true)] out Token? token, out int parsedLength)
    {
        token = null;
        parsedLength = default;

        if (formula.IsEmpty)
        {
            return false;
        }

        if (formula[0] is '+' or '-' or '*' or '/' or '!')
        {
            token = new Token(formula[0].ToString(), TokenType.Operation);
            parsedLength = 1;
        }
        else if (formula.StartsWith("||", StringComparison.Ordinal) ||
                 formula.StartsWith("&&", StringComparison.Ordinal))
        {
            token = new Token(formula[..2].ToString(), TokenType.Operation);
            parsedLength = 2;
        }
        else
        {
            return false;
        }

        return true;
    }
}