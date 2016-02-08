namespace Calculator.Core.Operation
{
    internal class UnaryMinusOperation : OperationBase
    {
        public UnaryMinusOperation()
            : base(OperationPriority.Unary, true)
        {
        }

        public override decimal GetResult()
        {
            return -this.leftOperand;
        }
    }
}
