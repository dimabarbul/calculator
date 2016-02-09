using System;
using Calculator.Core.Enum;
using Calculator.Core.Exception;
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

            switch (token.Text)
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
                case OperationToken.Floor:
                    operation = new FloorOperation();
                    break;
                case OperationToken.Ceil:
                    operation = new CeilOperation();
                    break;
                case OperationToken.Or:
                    operation = new OrOperation();
                    break;
                case OperationToken.And:
                    operation = new AndOperation();
                    break;
                case OperationToken.Not:
                    operation = new NotOperation();
                    break;
                default:
                    throw new CalculateException(CalculateExceptionCode.UnknownOperation, token.Text);
            }

            return operation;
        }

        private static string GetOperationClassName(string operation)
        {
            return string.Concat("Calculator.Core.Operation.", char.ToUpper(operation[0]), operation.Substring(1), "Operation");
        }

        private static class OperationToken
        {
            public const string Add = "+";
            public const string Subtract = "-";
            public const string Multiply = "*";
            public const string Divide = "/";
            public const string Floor = "floor";
            public const string Ceil = "ceil";
            public const string Or = "||";
            public const string And = "&&";
            public const string Not = "!";
        }
    }
}
