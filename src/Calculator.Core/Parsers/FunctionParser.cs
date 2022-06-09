using System.Diagnostics.CodeAnalysis;
using Calculator.Core.ParsingContexts;
using Calculator.Core.Tokens;

namespace Calculator.Core.Parsers;

public class FunctionParser : IParser
{
    private readonly Dictionary<string, Function> operations = new();

    public FunctionParser(IEnumerable<Operation> operations)
    {
        foreach (Operation operation in operations)
        {
            if (operation is Function function)
            {
                this.operations[function.FunctionName] = function;
            }
        }
    }

    public bool TryParse(ReadOnlySpan<char> formula, ParsingContext context, [NotNullWhen(true)] out Token? token,
        out int parsedLength)
    {
        token = null;
        parsedLength = default;

        Token? foundFunction = null;

        if (formula.IsEmpty)
        {
            return false;
        }

        foreach ((string text, Function function) in this.operations)
        {
            if (formula.StartsWith(text, StringComparison.Ordinal))
            {
                foundFunction = function;
                parsedLength = text.Length;
            }
        }

        if (foundFunction == null)
        {
            return false;
        }

        token = foundFunction;

        return true;
    }
}