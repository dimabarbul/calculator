using Calculator.Core.Enum;

namespace Calculator.Core.Operation
{
    internal class MultiplyOperation : DecimalOperationBase
    {
        public MultiplyOperation()
            : base(OperationPriority.Multiply, false)
        {
        }

        protected override decimal GetDecimalResult(decimal leftOperand, decimal? rightOperand)
        {
            return leftOperand * rightOperand.Value;
        }
    }
}
