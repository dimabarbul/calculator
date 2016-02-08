using System.Text.RegularExpressions;
using Calculator.Core.Enum;

namespace Calculator.Core.Parser
{
    internal abstract class RegexParser : IParser
    {
        protected string pattern;
        protected TokenType type;

        public RegexParser(string pattern, TokenType type)
        {
            this.pattern = pattern;
            this.type = type;
        }

        public Token TryParse(string formula, ref int startIndex)
        {
            Regex regex = new Regex(this.pattern);

            Match match = regex.Match(formula, startIndex);

            if (!match.Success || match.Index != startIndex)
            {
                return null;
            }

            startIndex += match.Value.Length;

            return new Token(match.Value, this.type);
        }
    }
}
