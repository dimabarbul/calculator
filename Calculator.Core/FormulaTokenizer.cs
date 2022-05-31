using System.Text.RegularExpressions;
using Calculator.Core.Enum;
using Calculator.Core.Exception;
using Calculator.Core.Parser;

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

        ReadOnlyMemory<char> sanitizedFormula = new Regex(@"\s+").Replace(formula, string.Empty).AsMemory();

        int index = 0;

        while (index < sanitizedFormula.Length)
        {
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