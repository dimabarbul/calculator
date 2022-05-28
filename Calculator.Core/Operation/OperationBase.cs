using Calculator.Core.Enum;

namespace Calculator.Core.Operation;

public abstract class OperationBase
{
    public OperationPriority Priority { get; }
    public int OperandsCount { get; }

    public OperationBase(OperationPriority priority, int operandsCount)
    {
        this.Priority = priority;
        this.OperandsCount = operandsCount;
    }

    public abstract Token Perform(params Token[] operands);
}