using Calculator.Core.Enums;

namespace Calculator.Core.Tokens;

public abstract class Operation : Token
{
    public OperationPriority Priority { get; }
    public int MaxOperandsCount { get; }
    public int MinOperandsCount { get; }

    protected Operation(OperationPriority priority, int maxOperandsCount, int? minOperandsCount = null)
    {
        this.Priority = priority;
        this.MaxOperandsCount = maxOperandsCount;
        this.MinOperandsCount = minOperandsCount ?? maxOperandsCount;
    }

    protected void ValidateOperandsCount(IList<Token> operands)
    {
        if (operands.Count < this.MinOperandsCount || operands.Count > this.MaxOperandsCount)
        {
            throw new ArgumentException($"Operands count is invalid. Expected: {this.MinOperandsCount} to {this.MaxOperandsCount}, got: {operands.Count}");
        }
    }

    public abstract Token Execute(IList<Token> operands);
}