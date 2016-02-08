using Calculator.Core.Enum;

namespace Calculator.Core.Parser
{
    internal class DecimalParser : RegexParser
    {
        public DecimalParser()
            : base(@"\d+(\.\d*)?|\.\d*", TokenType.Decimal)
        {
        }
    }
}
