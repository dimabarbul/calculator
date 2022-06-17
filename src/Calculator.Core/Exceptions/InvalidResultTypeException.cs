namespace Calculator.Core.Exceptions;

public class InvalidResultTypeException : CalculateException
{
    public InvalidResultTypeException(Type expectedType, Type actualType, Exception? innerException = null)
        : base($"Result is of type {actualType}, but expected {expectedType}", innerException)
    {
    }
}