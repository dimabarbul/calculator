using Calculator.Core.Enum;

namespace Calculator.Core.Operation
{
    internal class SubtractOperation : DecimalOperationBase
    {
        public SubtractOperation()
            : base(OperationPriority.Subtract, false)
        {
        }

        protected override decimal GetDecimalResult(decimal leftOperand, decimal? rightOperand)
        {
            return leftOperand - rightOperand.Value;
        }
    }
}
