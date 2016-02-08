using System;
using Calculator.Core.Operation;

namespace Calculator.Core
{
    internal static class OperationFactory
    {
        /// <summary>
        /// Creates operation based on token and flag that operation is unary.
        /// </summary>
        /// <param name="token">Token to create operation from.</param>
        /// <param name="isUnary">
        /// True if unary version should be created.
        /// Makes sense only for unary "+" and unary "-".
        /// </param>
        /// <returns>The operation.</returns>
        public static OperationBase Create(Token token, bool isUnary = false)
        {
            OperationBase operation;

            switch (token.Value)
            {
                case OperationToken.Add:
                    if (isUnary)
                    {
                        operation = new UnaryPlusOperation();
                    }
                    else
                    {
                        operation = new AddOperation();
                    }

                    break;
                case OperationToken.Subtract:
                    if (isUnary)
                    {
                        operation = new UnaryMinusOperation();
                    }
                    else
                    {
                        operation = new SubtractOperation();
                    }

                    break;
                case OperationToken.Multiply:
                    operation = new MultiplyOperation();
                    break;
                case OperationToken.Divide:
                    operation = new DivideOperation();
                    break;
                default:
                    throw new NotImplementedException(string.Format(
                        @"Operation is not defined for token ""{0}"".", token.Value));
            }

            return operation;
        }
    }
}
