using System;

namespace Calculator.Core.Operation
{
    internal class SubtractOperation : OperationBase
    {
        public override decimal GetResult()
        {
            return this.leftOperand - this.rightOperand;
        }
    }
}
