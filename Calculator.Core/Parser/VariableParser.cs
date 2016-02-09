using Calculator.Core.Enum;

namespace Calculator.Core.Parser
{
    internal class VariableParser : RegexParser
    {
        public VariableParser()
            : base(@"(?!true|false)[a-zA-Z_][a-zA-Z_0-9]*(?![(\[{<a-zA-Z0-9_])", TokenType.Variable)
        {
        }
    }
}
