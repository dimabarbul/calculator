using System.Diagnostics.CodeAnalysis;

namespace Calculator.Core.Parser;

public interface IParser
{
    bool TryParse(ReadOnlySpan<char> formula, [NotNullWhen(true)] out Token? token, out int parsedLength);
}