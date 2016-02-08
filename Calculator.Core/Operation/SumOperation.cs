namespace Calculator.Core.Operation
{
    internal class SumOperation : OperationBase
    {
        public override decimal GetResult()
        {
            return this.leftOperand + this.rightOperand;
        }
    }
}
