﻿namespace Calculator.Core.Operation
{
    internal class DivideOperation : OperationBase
    {
        public override decimal GetResult()
        {
            return this.leftOperand / this.rightOperand;
        }

        public override OperationPriority Priority
        {
            get { return OperationPriority.Divide; }
        }
    }
}