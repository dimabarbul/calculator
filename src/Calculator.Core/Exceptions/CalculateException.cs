using Calculator.Core.Enums;

namespace Calculator.Core.Exceptions;

public class CalculateException : ExceptionWithCode
{
    public CalculateException(CalculateExceptionCode code, string message)
        : base((int)code, message)
    {
    }
}