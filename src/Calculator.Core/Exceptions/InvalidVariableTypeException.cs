using Calculator.Core.Tokens;

namespace Calculator.Core.Exceptions;

public class InvalidVariableTypeException : CalculateException
{
    public InvalidVariableTypeException(Variable variable, Exception? innerException = null)
        : base($"Invalid type of variable {variable.Name}", innerException)
    {
    }
}