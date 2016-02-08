namespace Calculator.Core.Operation
{
    internal abstract class OperationBase
    {
        protected decimal leftOperand;
        protected decimal rightOperand;

        public void SetOperands(decimal leftOperand, decimal rightOperand)
        {
            this.leftOperand = leftOperand;
            this.rightOperand = rightOperand;
        }

        public abstract decimal GetResult();
        public abstract OperationPriority Priority { get; }
    }
}
