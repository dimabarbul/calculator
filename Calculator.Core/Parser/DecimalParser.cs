using Calculator.Core.Enum;

namespace Calculator.Core.Parser
{
    public class DecimalParser : RegexParser
    {
        public DecimalParser()
            : base(@"\d+(\.\d*)?|\.\d*", TokenType.Decimal)
        {
        }
    }
}
