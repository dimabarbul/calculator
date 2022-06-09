namespace Calculator.Core.Tokens;

public sealed class NullToken : Token
{
    public static readonly NullToken Instance = new();

    private NullToken()
    {
    }
}