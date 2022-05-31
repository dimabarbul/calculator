using System.Diagnostics.CodeAnalysis;
using Calculator.Core.Operation;

namespace Calculator.Core.Parser;

public class OperatorParser : IParser
{
    private readonly Dictionary<string, Operator> operations;

    public OperatorParser(IEnumerable<Operation.Operation> operations)
    {
        this.operations = operations
            .Where(o => o is Operator)
            .Cast<Operator>()
            .ToDictionary(o => o.Text);
    }

    public bool TryParse(ReadOnlySpan<char> formula, [NotNullWhen(true)] out Token? token, out int parsedLength)
    {
        token = null;
        parsedLength = default;

        if (formula.IsEmpty)
        {
            return false;
        }

        foreach ((string text, Operation.Operation @operator) in this.operations)
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