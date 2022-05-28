using Calculator.Core.Enum;

namespace Calculator.Core.Parser
{
    public class BoolParser : RegexParser
    {
        public BoolParser()
            : base(@"true|false", TokenType.Bool)
        {
        }
    }
}
