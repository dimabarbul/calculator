using Calculator.Core.Enum;

namespace Calculator.Core.Operation;

public abstract class Function : Operation
{
    protected Function(OperationPriority priority, int operandsCount, int? minOperandsCount = null)
        : base(priority, operandsCount, minOperandsCount)
    {
    }

    public abstract string FunctionName { get; }
}