namespace Calculator.Core.Exceptions;

public class UnparsedTokenException : ParseException
{
    public UnparsedTokenException(string formula, int index, Exception? innerException = null)
        : base($"Unparsed token in formula {formula} at {index}", innerException)
    {
    }
}
