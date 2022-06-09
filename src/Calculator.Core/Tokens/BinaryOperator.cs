namespace Calculator.Core.Tokens;

public abstract class BinaryOperator : Operation
{
    protected BinaryOperator(int priority)
        : base(priority, 2)
    {
    }

    public abstract string Text { get; }
}