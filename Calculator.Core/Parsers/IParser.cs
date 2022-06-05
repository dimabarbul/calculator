using System.Diagnostics.CodeAnalysis;
using Calculator.Core.Tokens;

namespace Calculator.Core.Parsers;

public interface IParser
{
    bool TryParse(ReadOnlySpan<char> formula, [NotNullWhen(true)] out Token? token, out int parsedLength);
}