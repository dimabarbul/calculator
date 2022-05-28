namespace Calculator.Core.Exception;

public abstract class ExceptionWithCode : System.Exception
{
    public int Code { get; }

    protected ExceptionWithCode(int code, string? message = null)
        : base(message)
    {
        this.Code = code;
    }
}