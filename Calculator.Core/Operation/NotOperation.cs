using Calculator.Core.Enum;

namespace Calculator.Core.Operation
{
    internal class NotOperation : BoolOperationBase
    {
        public NotOperation()
            : base(OperationPriority.Unary, true)
        {
        }

        protected override bool GetBoolResult(bool leftOperand, bool? rightOperand)
        {
            return !leftOperand;
        }
    }
}
