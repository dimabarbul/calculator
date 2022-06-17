namespace Calculator.Core.Exceptions;

public class UnexpectedFormulaEndException : ParseException
{
    public UnexpectedFormulaEndException(string formula, Exception? innerException = null)
        : base($"Unexpected end of formula {formula}", innerException)
    {
    }
}