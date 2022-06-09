using System.Diagnostics.CodeAnalysis;
using Calculator.Core.ParsingContexts;
using Calculator.Core.Tokens;

namespace Calculator.Core.Parsers;

public interface IParser
{
    bool TryParse(
        ReadOnlySpan<char> formula,
        ParsingContext context,
        [NotNullWhen(true)] out Token? token,
        out int parsedLength);
}