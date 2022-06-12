using System.Diagnostics.CodeAnalysis;
using Calculator.Core.Operands;
using Calculator.Core.Parsers;
using Calculator.Core.ParsingContexts;
using Calculator.Core.Tokens;

namespace Calculator.Extra.Parsers;

public class DecimalOperandParser : IParser
{
    private const string Zero = "0";

    public bool TryParse(ReadOnlySpan<char> formula, ParsingContext context, [NotNullWhen(true)] out Token? token,
        out int parsedLength)
    {
        token = null;
        parsedLength = default;

        int index = 0;
        bool decimalPointEncountered = false;
        int integerRepresentation = 0;
        byte decimalPlaces = 0;
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
            else
            {
                if (decimalPointEncountered)
                {
                    decimalPlaces++;
                }

                integerRepresentation = integerRepresentation * 10 + (formula[index] - '0');
            }

            index++;
        }

        if (index == 0)
        {
            return false;
        }

        token = new Operand<decimal>(new decimal(integerRepresentation, 0, 0, false, decimalPlaces));
        parsedLength = index;

        return true;
    }
}