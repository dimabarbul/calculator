using Calculator.Core.Enum;

namespace Calculator.Core.Operation;

public abstract class Operator : Operation
{
    protected Operator(OperationPriority priority, int operandsCount, int? minOperandsCount = null)
        : base(priority, operandsCount, minOperandsCount)
    {
    }

    public abstract string Text { get; }
}