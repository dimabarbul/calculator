using System.Diagnostics.CodeAnalysis;

namespace Calculator.Core.Parsers;

public interface IParser
{
    bool TryParse(ReadOnlySpan<char> formula, [NotNullWhen(true)] out Token? token, out int parsedLength);
}