using Calculator.Core.Enum;

namespace Calculator.Core.Exception
{
    internal class CalculateException : ExceptionWithCode
    {
        public CalculateException(CalculateExceptionCode code, string message = null)
            : base((int)code, message)
        {
        }
    }
}
