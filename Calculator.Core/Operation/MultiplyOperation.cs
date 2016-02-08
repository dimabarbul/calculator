using Calculator.Core.Enum;

namespace Calculator.Core.Operation
{
    internal class MultiplyOperation : OperationBase
    {
        public MultiplyOperation()
            : base(OperationPriority.Multiply, false)
        {
        }

        public override decimal GetResult()
        {
            return this.leftOperand * this.rightOperand;
        }
    }
}
