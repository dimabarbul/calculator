using Calculator.Core.Tokens;

namespace Calculator.Core.Exceptions;

public class TokenNotAllowedInContextException : Exception
{
    public Token Token { get; }

    public TokenNotAllowedInContextException(Token token, Exception? innerException = null)
        : base($"Token {token} is not allowed in current context", innerException)
    {
        this.Token = token;
    }
}