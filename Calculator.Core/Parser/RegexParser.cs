using System.Text.RegularExpressions;
using Calculator.Core.Enum;

namespace Calculator.Core.Parser
{
    public abstract class RegexParser : IParser
    {
        protected string pattern;
        protected TokenType type;

        public RegexParser(string pattern, TokenType type)
        {
            this.pattern = pattern;
            this.type = type;
        }

        public int TryParse(string formula, out Token token, int startIndex = 0)
        {
            token = null;
        
            Regex regex = new Regex(this.pattern, RegexOptions.IgnoreCase);
            Match match = regex.Match(formula, startIndex);

            if (!match.Success || match.Index != startIndex)
            {
                return 0;
            }

            token = new Token(match.Value, this.type);

            return match.Value.Length;
        }
    }
}
