namespace Calculator.Core.Operation
{
    internal class UnaryPlusOperation : OperationBase
    {
        public UnaryPlusOperation()
            : base(OperationPriority.Unary, true)
        {
        }

        public override decimal GetResult()
        {
            return this.leftOperand;
        }
    }
}
