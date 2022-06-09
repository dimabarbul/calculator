using System.Diagnostics.CodeAnalysis;
using Calculator.Core.ParsingContexts;
using Calculator.Core.Tokens;

namespace Calculator.Core.Parsers;

public class OperatorParser : IParser
{
    private readonly Dictionary<string, Operation> binaryOperators = new();
    private readonly Dictionary<string, Operation> unaryOperators = new();

    public OperatorParser(IEnumerable<Operation> operations)
    {
        foreach (Operation operation in operations)
        {
            if (operation is BinaryOperator binaryOperator)
            {
                this.binaryOperators[binaryOperator.Text] = binaryOperator;
            }
            else if (operation is UnaryOperator unaryOperator)
            {
                this.unaryOperators[unaryOperator.Text] = unaryOperator;
            }
        }
    }

    public bool TryParse(ReadOnlySpan<char> formula, ParsingContext context, [NotNullWhen(true)] out Token? token,
        out int parsedLength)
    {
        token = null;
        parsedLength = default;

        if (formula.IsEmpty)
        {
            return false;
        }

        Dictionary<string, Operation> operators = context.IsBinaryOperatorAllowed ?
            this.binaryOperators :
            this.unaryOperators;
        foreach ((string text, Operation @operator) in operators)
        {
            if (formula.StartsWith(text, StringComparison.Ordinal))
            {
                token = @operator;
                parsedLength = text.Length;

                return true;
            }
        }

        return false;
    }
}