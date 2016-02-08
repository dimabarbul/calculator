using Calculator.Core.Enum;

namespace Calculator.Core.Operation
{
    internal class DivideOperation : DecimalOperationBase
    {
        public DivideOperation()
            : base(OperationPriority.Divide, false)
        {
        }

        protected override decimal GetDecimalResult(decimal leftOperand, decimal? rightOperand)
        {
            return leftOperand / rightOperand.Value;
        }
    }
}
