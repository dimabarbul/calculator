namespace Calculator.Core.Operation
{
    internal class SumOperation : OperationBase
    {
        public override decimal GetResult()
        {
            return this.leftOperand + this.rightOperand;
        }

        public override OperationPriority Priority
        {
            get { return OperationPriority.Sum; }
        }
    }
}
