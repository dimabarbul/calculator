namespace Calculator.Core.Exceptions;

public abstract class ExceptionWithCode : Exception
{
    public int Code { get; }

    protected ExceptionWithCode(int code, string? message = null)
        : base(message)
    {
        this.Code = code;
    }
}