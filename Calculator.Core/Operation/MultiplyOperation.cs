namespace Calculator.Core.Operation
{
    internal class MultiplyOperation : OperationBase
    {
        public override decimal GetResult()
        {
            return this.leftOperand * this.rightOperand;
        }
    }
}
