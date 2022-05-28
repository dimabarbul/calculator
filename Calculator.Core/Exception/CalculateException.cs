using Calculator.Core.Enum;

namespace Calculator.Core.Exception;

public class CalculateException : ExceptionWithCode
{
    public CalculateException(CalculateExceptionCode code, string message = null)
        : base((int)code, message)
    {
    }
}