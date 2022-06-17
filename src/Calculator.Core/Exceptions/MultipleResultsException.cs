namespace Calculator.Core.Exceptions;

public class MultipleResultsException : CalculateException
{
    public MultipleResultsException(string formula, int actualResultsCount, Exception? innerException = null)
        : base($"Expected formula {formula} to produce single result, but got {actualResultsCount}", innerException)
    {
    }
}