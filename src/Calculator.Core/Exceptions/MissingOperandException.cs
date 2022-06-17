namespace Calculator.Core.Exceptions;

public class MissingOperandException : CalculateException
{
    public MissingOperandException(int expectedCount, int actualCount, Exception? innerException = null)
        : base($"Operation expected {expectedCount} operands, but there are only {actualCount} left", innerException)
    {
    }
}