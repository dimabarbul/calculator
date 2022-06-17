namespace Calculator.Core.Exceptions;

public abstract class CalculateException : Exception
{
    protected CalculateException(string message, Exception? innerException = null)
        : base(message, innerException)
    {
    }
}