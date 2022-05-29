using System.Diagnostics.CodeAnalysis;
using Calculator.Core.Enum;

namespace Calculator.Core.Parser;

public class DecimalParser : IParser
{
    public bool TryParse(ReadOnlySpan<char> formula, [NotNullWhen(true)] out Token? token, out int parsedLength)
    {
        token = null;
        parsedLength = default;

        int index = 0;
        bool decimalPointEncountered = false;
        while (index < formula.Length)
        {
            if (formula[index] == '.')
            {
                if (decimalPointEncountered)
                {
                    break;
                }

                decimalPointEncountered = true;
            }
            else if (!char.IsDigit(formula[index]))
            {
                break;
            }

            index++;
        }

        if (index == 0)
        {
            return false;
        }

        token = new Token(formula[..index].ToString(), TokenType.Decimal);
        parsedLength = index;

        return true;
    }
}