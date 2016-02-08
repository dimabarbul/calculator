using Calculator.Core.Enum;

namespace Calculator.Core.Parser
{
    internal class BoolParser : RegexParser
    {
        public BoolParser()
            : base(@"true|false", TokenType.Bool)
        {
        }
    }
}
