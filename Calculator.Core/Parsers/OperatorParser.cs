using System.Diagnostics.CodeAnalysis;
using Calculator.Core.Tokens;

namespace Calculator.Core.Parsers;

public class OperatorParser : IParser
{
    private readonly Dictionary<string, Operator> operations = new();

    public OperatorParser(IEnumerable<Operation> operations)
    {
        foreach (Operation operation in operations)
        {
            if (operation is Operator @operator)
            {
                this.operations[@operator.Text] = @operator;
            }
        }
    }

    public bool TryParse(ReadOnlySpan<char> formula, [NotNullWhen(true)] out Token? token, out int parsedLength)
    {
        token = null;
        parsedLength = default;

        if (formula.IsEmpty)
        {
            return false;
        }

        foreach ((string text, Operation @operator) in this.operations)
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