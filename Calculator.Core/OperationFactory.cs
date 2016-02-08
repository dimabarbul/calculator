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
                case OperationToken.Add:
                    operation = new AddOperation();
                    break;
                case OperationToken.Subtract:
                    operation = new SubtractOperation();
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
