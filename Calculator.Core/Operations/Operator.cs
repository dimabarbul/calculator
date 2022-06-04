using Calculator.Core.Enums;

namespace Calculator.Core.Operations;

public abstract class Operator : Operation
{
    protected Operator(OperationPriority priority, int operandsCount, int? minOperandsCount = null)
        : base(priority, operandsCount, minOperandsCount)
    {
    }

    public abstract string Text { get; }
}