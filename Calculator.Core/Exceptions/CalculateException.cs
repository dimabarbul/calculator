using Calculator.Core.Enums;

namespace Calculator.Core.Exceptions;

public class CalculateException : ExceptionWithCode
{
    public CalculateException(CalculateExceptionCode code, string? message = null)
        : base((int)code, message)
    {
    }
}