namespace Calculator.Core.Tokens;

public abstract class Operator : Operation
{
    protected Operator(int priority, int operandsCount, int? minOperandsCount = null)
        : base(priority, operandsCount, minOperandsCount)
    {
    }

    public abstract string Text { get; }
}