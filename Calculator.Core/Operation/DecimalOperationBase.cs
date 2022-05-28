using System.Globalization;
using Calculator.Core.Enum;

namespace Calculator.Core.Operation;

internal abstract class DecimalOperationBase : OperationBase
{
    public DecimalOperationBase(OperationPriority priority, int operandsCount)
        : base(priority, operandsCount)
    {
    }

    public override Token Perform(params Token[] operands)
    {
        decimal result = this.GetDecimalResult(
            operands.Select(o => o.GetValue<decimal>()).ToArray()
        );
        Token token = new(result.ToString(CultureInfo.InvariantCulture), TokenType.Decimal);

        return token;
    }

    protected abstract decimal GetDecimalResult(params decimal[] operands);
}