using System;
using Calculator.Core.Enum;

namespace Calculator.Core.Operation
{
    internal class CeilOperation : OperationBase
    {
        public CeilOperation()
            : base(OperationPriority.Unary, true)
        {
        }

        public override decimal GetResult()
        {
            return Math.Ceiling(this.leftOperand);
        }
    }
}
