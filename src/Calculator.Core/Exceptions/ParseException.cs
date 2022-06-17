namespace Calculator.Core.Exceptions;

public abstract class ParseException : Exception
{
    protected ParseException(string message, Exception? innerException = null)
        : base(message, innerException)
    {
    }
}
