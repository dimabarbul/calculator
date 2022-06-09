namespace Calculator.Core.Tokens;

public abstract class UnaryOperator : Operation
{
    protected UnaryOperator()
        : base(HighestPriority, 1, isLeftToRight: false)
    {
    }

    public override Token Execute(IList<Token> operands)
    {
        if (operands.Count > 1)
        {
            throw new ArgumentException($"Unary operator {this.Text} expected one argument, got {operands.Count}");
        }

        if (operands[0] is not Operand operand)
        {
            throw new ArgumentException($"Unary operator can be performed only on operands, got {operands[0].GetType()}");
        }

        return this.ExecuteUnaryOperator(operand);
    }

    public abstract string Text { get; }

    protected abstract Operand ExecuteUnaryOperator(Operand operand);
}