using Calculator.Core.Enum;

namespace Calculator.Core.Parser
{
    internal class OperationParser : RegexParser
    {
        public OperationParser()
            : base(@"[^\da-zA-Z()\[\]{}<>.]+|[a-zA-Z_][a-zA-Z0-9_]*(?=[(\[{<])", TokenType.Operation)
        {
        }
    }
}
