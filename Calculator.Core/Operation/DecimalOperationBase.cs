using Calculator.Core.Enum;

namespace Calculator.Core.Operation;

internal abstract class DecimalOperationBase : OperationBase
{
    public DecimalOperationBase(OperationPriority priority, bool isUnary)
        : base(priority, isUnary)
    {
    }

    public override Token GetResult()
    {
        decimal result = this.GetDecimalResult(
            this.leftOperand.GetValue<decimal>(),
            this.rightOperand == null ? (decimal?)null : this.rightOperand.GetValue<decimal>()
        );
        Token token = new(result.ToString(), TokenType.Decimal);

        return token;
    }

    protected abstract decimal GetDecimalResult(decimal leftOperand, decimal? rightOperand);
}