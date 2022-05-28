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

        string sanitizedFormula = (new Regex(@"\s+")).Replace(formula, string.Empty);

        int index = 0;
        Token? token = null;

        while (index < sanitizedFormula.Length)
        {
            foreach (IParser parser in this.parsers)
            {
                index += parser.TryParse(sanitizedFormula, out token, index);

                if (token != null)
                {
                    break;
                }
            }

            if (token == null)
            {
                throw new ParseException(ParseExceptionCode.UnparsedToken);
            }

            yield return token;
        }
    }

    public TokenType DetectTokenType(object value)
    {
        Token? token = null;
        string stringValue = value.ToString();
        int tokenLength = 0;

        foreach (IParser parser in this.parsers)
        {
            tokenLength = parser.TryParse(stringValue, out token);

            if (token != null)
            {
                break;
            }
        }

        if (token == null)
        {
            throw new ParseException(ParseExceptionCode.UnparsedToken);
        }

        if (stringValue.Length != tokenLength)
        {
            throw new ParseException(ParseExceptionCode.UnparsedToken);
        }

        return token.Type;
    }

    public bool IsValueTokenType(TokenType type)
    {
        return type == TokenType.Bool || type == TokenType.Decimal;
    }
}