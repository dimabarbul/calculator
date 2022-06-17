using Calculator.Core.Tokens;

namespace Calculator.Core.Exceptions;

public class UnknownVariableException : CalculateException
{
    public UnknownVariableException(Variable variable, Exception? innerException = null)
        : base($"Unknown variable {variable.Name}", innerException)
    {
    }
}