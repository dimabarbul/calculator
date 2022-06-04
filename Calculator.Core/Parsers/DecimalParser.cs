using System.Diagnostics.CodeAnalysis;
using Calculator.Core.Operands;

namespace Calculator.Core.Parsers;

public class DecimalParser : IParser
{
    private const string Zero = "0";

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

        string tokenText = index == 1 && formula[0] == '.' ? Zero : formula[..index].ToString();
        token = new Operand<decimal>(decimal.Parse(tokenText));
        parsedLength = index;

        return true;
    }
}