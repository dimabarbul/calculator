using Calculator.Core.Enum;

namespace Calculator.Core.Operation
{
    internal class AddOperation : OperationBase
    {
        public AddOperation()
            : base(OperationPriority.Add, false)
        {
        }

        public override decimal GetResult()
        {
            return this.leftOperand + this.rightOperand;
        }
    }
}
