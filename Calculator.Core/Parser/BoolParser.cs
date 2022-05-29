using System.Diagnostics.CodeAnalysis;
using Calculator.Core.Enum;

namespace Calculator.Core.Parser;

public class BoolParser : IParser
{
    private const string TrueString = "true";
    private const string FalseString = "false";

    public bool TryParse(ReadOnlySpan<char> formula, [NotNullWhen(true)] out Token? token, out int parsedLength)
    {
        token = null;
        parsedLength = default;

        if (formula.StartsWith(TrueString, StringComparison.OrdinalIgnoreCase))
        {
            token = new Token(TrueString, TokenType.Bool);
            parsedLength = TrueString.Length;

            return true;
        }

        if (formula.StartsWith(FalseString, StringComparison.OrdinalIgnoreCase))
        {
            token = new Token(FalseString, TokenType.Bool);
            parsedLength = FalseString.Length;

            return true;
        }

        return false;
    }
}