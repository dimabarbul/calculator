using System.Collections.Generic;
using System.Text.RegularExpressions;
using Calculator.Core.Enum;
using Calculator.Core.Exception;
using Calculator.Core.Parser;

namespace Calculator.Core
{
    public static class FormulaTokenizer
    {
        private static readonly IParser[] Parsers = new IParser[]
        {
            new DecimalParser(),
            new SubformulaParser(),
            new BoolParser(),
            new OperationParser(),
            new VariableParser(),
        };

        public static IEnumerable<Token> GetTokens(string formula)
        {
            if (string.IsNullOrWhiteSpace(formula))
            {
                yield break;
            }

            string sanitizedFormula = (new Regex(@"\s*")).Replace(formula, string.Empty);

            int index = 0;
            Token token = null;

            while (index < sanitizedFormula.Length)
            {
                foreach (IParser parser in Parsers)
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

        public static TokenType DetectTokenType(object value)
        {
            Token token = null;
            string stringValue = value.ToString();
            int tokenLength = 0;

            foreach (IParser parser in Parsers)
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

        public static bool IsValueTokenType(TokenType type)
        {
            return type == TokenType.Bool || type == TokenType.Decimal;
        }
    }
}
