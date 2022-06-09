using Calculator.Core.Enums;

namespace Calculator.Core.Exceptions;

public class ParseException : ExceptionWithCode
{
    public ParseException(ParseExceptionCode code, string formula, int index)
        : this(code, $"Cannot parse formula {formula}, last position: {index}")
    {
    }

    public ParseException(ParseExceptionCode code, string message)
        : base((int)code, message)
    {
    }
}