using Calculator.Core.Enum;

namespace Calculator.Core.Operation;

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

    protected void ValidateOperandsCount(Token[] operands)
    {
        if (operands.Length < this.MinOperandsCount || operands.Length > this.MaxOperandsCount)
        {
            throw new ArgumentException($"Operands count is invalid. Expected: {this.MinOperandsCount} to {this.MaxOperandsCount}, got: {operands.Length}");
        }
    }

    public abstract Token Execute(params Token[] operands);
}