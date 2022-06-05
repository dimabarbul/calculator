using System.Linq;
using Calculator.Core.Enums;
using Calculator.Core.Exceptions;
using Calculator.Core.Parsers;
using Calculator.Core.Tokens;

namespace Calculator.Core;

public class FormulaTokenizer
{
    private readonly IParser[] parsers;

    public FormulaTokenizer(IEnumerable<IParser> parsers)
    {
        this.parsers = parsers.ToArray();
    }

    public IEnumerable<Token> GetTokens(string formula)
    {
        if (string.IsNullOrWhiteSpace(formula))
        {
            yield break;
        }

        ReadOnlyMemory<char> sanitizedFormula = formula.AsMemory();

        int index = 0;

        while (index < sanitizedFormula.Length)
        {
            if (char.IsWhiteSpace(sanitizedFormula.Span[index]))
            {
                index++;
                continue;
            }

            Token? token = null;
            foreach (IParser parser in this.parsers)
            {
                if (parser.TryParse(sanitizedFormula[index..].Span, out token, out int parsedLength))
                {
                    index += parsedLength;
                    break;
                }
            }

            if (token == null)
            {
                throw new ParseException(ParseExceptionCode.UnparsedToken, formula, index);
            }

            yield return token;
        }
    }
}