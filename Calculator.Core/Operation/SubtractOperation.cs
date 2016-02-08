using Calculator.Core.Enum;

namespace Calculator.Core.Operation
{
    internal class SubtractOperation : OperationBase
    {
        public SubtractOperation()
            : base(OperationPriority.Subtract, false)
        {
        }

        public override decimal GetResult()
        {
            return this.leftOperand - this.rightOperand;
        }
    }
}
