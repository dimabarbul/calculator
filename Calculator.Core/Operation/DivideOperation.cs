using Calculator.Core.Enum;

namespace Calculator.Core.Operation
{
    internal class DivideOperation : OperationBase
    {
        public DivideOperation()
            : base(OperationPriority.Divide, false)
        {
        }

        public override decimal GetResult()
        {
            return this.leftOperand / this.rightOperand;
        }
    }
}
