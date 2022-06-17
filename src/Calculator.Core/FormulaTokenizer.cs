using System.Linq;
using Calculator.Core.Exceptions;
using Calculator.Core.Parsers;
using Calculator.Core.ParsingContexts;
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
        ParsingContext parsingContext = ParsingContext.Initial;

        if (string.IsNullOrWhiteSpace(formula))
        {
            yield break;
        }

        ReadOnlyMemory<char> formulaInMemory = formula.AsMemory();

        int index = 0;

        while (index < formulaInMemory.Length)
        {
            if (char.IsWhiteSpace(formulaInMemory.Span[index]))
            {
                index++;
                continue;
            }

            Token? token = null;
            foreach (IParser parser in this.parsers)
            {
                if (parser.TryParse(formulaInMemory[index..].Span, parsingContext, out token, out int parsedLength))
                {
                    index += parsedLength;
                    break;
                }
            }

            if (token == null)
            {
                throw new UnparsedTokenException(formula, index);
            }

            try
            {
                parsingContext = parsingContext.SetNextToken(token);
            }
            catch (TokenNotAllowedInContextException e)
            {
                throw new MisplacedTokenException(formula, index, e.Token, e);
            }

            yield return token;
        }

        if (!parsingContext.IsEndAllowed)
        {
            throw new UnexpectedFormulaEndException(formula);
        }
    }
}