using Calculator.Core.Enum;

namespace Calculator.Core.Exception;

public class ParseException : ExceptionWithCode
{
    public ParseException(ParseExceptionCode code, string formula, int index)
        : base((int)code, $"Cannot parse formula {formula}, last position: {index}")
    {
    }
}