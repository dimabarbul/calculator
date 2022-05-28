using Calculator.Core.Enum;

namespace Calculator.Core.Exception;

public class ParseException : ExceptionWithCode
{
    public ParseException(ParseExceptionCode code)
        : base((int)code)
    {
    }
}