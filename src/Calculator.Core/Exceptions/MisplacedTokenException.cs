using Calculator.Core.Tokens;

namespace Calculator.Core.Exceptions;

public class MisplacedTokenException : ParseException
{
    public MisplacedTokenException(string formula, int index, Token token, Exception? innerException = null)
        : base($"Token {token} is unexpected in formula {formula} at {index}", innerException)
    {
    }
}
