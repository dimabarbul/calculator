using System.Diagnostics.CodeAnalysis;
using Calculator.Core.Operands;

namespace Calculator.Core.Parsers;

public class BoolParser : IParser
{
    private const string TrueString = "true";
    private const string FalseString = "false";

    private static readonly Operand<bool> True = new(true);
    private static readonly Operand<bool> False = new(false);

    public bool TryParse(ReadOnlySpan<char> formula, [NotNullWhen(true)] out Token? token, out int parsedLength)
    {
        token = null;
        parsedLength = default;

        if (formula.StartsWith(TrueString, StringComparison.OrdinalIgnoreCase))
        {
            token = True;
            parsedLength = TrueString.Length;

            return true;
        }

        if (formula.StartsWith(FalseString, StringComparison.OrdinalIgnoreCase))
        {
            token = False;
            parsedLength = FalseString.Length;

            return true;
        }

        return false;
    }
}