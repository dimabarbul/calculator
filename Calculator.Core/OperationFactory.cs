using System;
using Calculator.Core.Operation;

namespace Calculator.Core
{
    internal static class OperationFactory
    {
        public static OperationBase Create(Token token)
        {
            OperationBase operation;

            switch (token.Value)
            {
                case OperationToken.OperationSum:
                    operation = new SumOperation();
                    break;
                case OperationToken.OperationSubtract:
                    operation = new SubtractOperation();
                    break;
                default:
                    throw new NotImplementedException(string.Format(
                        "Operation is not defined for token {0}.", token));
            }

            return operation;
        }
    }
}
