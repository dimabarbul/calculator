using Calculator.Core.Enum;

namespace Calculator.Core.Operation;

internal abstract class BoolOperationBase : OperationBase
{
    public BoolOperationBase(OperationPriority priority, int operandsCount)
        : base(priority, operandsCount)
    {
    }

    public override Token Perform(params Token[] operands)
    {
        bool result = this.GetBoolResult(
            operands.Select(o => o.GetValue<bool>()).ToArray()
        );

        Token token = new(result.ToString(), TokenType.Bool);

        return token;
    }

    protected abstract bool GetBoolResult(params bool[] operands);
}