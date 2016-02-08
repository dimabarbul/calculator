using Calculator.Core.Enum;

namespace Calculator.Core.Operation
{
    internal abstract class BoolOperationBase : OperationBase
    {
        public BoolOperationBase(OperationPriority priority, bool isUnary)
            : base(priority, isUnary)
        {
        }

        public override Token GetResult()
        {
            bool result = this.GetBoolResult(
                this.leftOperand.GetValue<bool>(),
                this.rightOperand == null ? (bool?)null : this.rightOperand.GetValue<bool>()
            );

            Token token = new Token(result.ToString(), TokenType.Bool);

            return token;
        }

        protected abstract bool GetBoolResult(bool leftOperand, bool? rightOperand);
    }
}
