namespace Calculator.Core.Tokens;

public abstract class Operation : Token
{
    protected const int LowestPriority = 0;
    protected const int HighestPriority = 0;

    public int Priority { get; }
    public int OperandsCount { get; }
    public bool IsLeftToRight { get; }

    protected Operation(int priority, int operandsCount, bool isLeftToRight = true)
    {
        this.Priority = priority;
        this.OperandsCount = operandsCount;
        this.IsLeftToRight = isLeftToRight;
    }

    protected void ValidateOperandsCount(IList<Token> operands)
    {
        if (operands.Count != this.OperandsCount)
        {
            throw new ArgumentException($"Operands count is invalid. Expected: {this.OperandsCount}, got: {operands.Count}");
        }
    }

    public abstract Token Execute(IList<Token> operands);
}