using System;
using Calculator.Core.Enum;

namespace Calculator.Core.Operation
{
    internal class FloorOperation : OperationBase
    {
        public FloorOperation()
            : base(OperationPriority.Unary, true)
        {
        }

        public override decimal GetResult()
        {
            return Math.Floor(this.leftOperand);
        }
    }
}
