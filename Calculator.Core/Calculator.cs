using System;
using System.Collections.Generic;
using Calculator.Core.Enum;
using Calculator.Core.Exception;
using Calculator.Core.Operation;

namespace Calculator.Core
{
    /// <summary>
    /// Provides functionality to calculate formula values.
    /// </summary>
    public static class Calculator
    {
        public static TResult Calculate<TResult>(string formula)
        {
            if (string.IsNullOrWhiteSpace(formula))
            {
                throw new ArgumentNullException();
            }

            Stack<Token> operands = new Stack<Token>();
            Stack<OperationBase> operations = new Stack<OperationBase>();
            Token resultToken;

            foreach (Token token in FormulaTokenizer.GetTokens(formula))
            {
                if (token.Type == TokenType.Decimal || token.Type == TokenType.Bool)
                {
                    operands.Push(token);
                }
                else if (token.Type == TokenType.Subformula)
                {
                    resultToken = Calculator.Calculate<Token>(token.Text);

                    operands.Push(resultToken);
                }
                else
                {
                    OperationBase operation = OperationFactory.Create(token, operands.Count == 0);

                    while (operations.Count > 0)
                    {
                        OperationBase previousOperation = operations.Peek();

                        if (previousOperation.Priority >= operation.Priority)
                        {
                            ExecuteOperation(ref operands, ref operations);
                        }
                        else
                        {
                            break;
                        }
                    }

                    operations.Push(operation);
                }
            }

            while (operations.Count > 0)
            {
                ExecuteOperation(ref operands, ref operations);
            }

            if (operands.Count != 1)
            {
                throw new CalculateException(CalculateExceptionCode.NotSingleResult);
            }

            resultToken = operands.Pop();

            TResult result;

            if (typeof(Token) == typeof(TResult))
            {
                result = (TResult)Convert.ChangeType(resultToken, typeof(TResult));
            }
            else
            {
                result = resultToken.GetValue<TResult>();
            }

            return result;
        }

        /// <summary>
        /// Calculates provided formula.
        /// </summary>
        /// <param name="formula">Formula to calculate.</param>
        /// <returns>Calculation result.</returns>
        public static decimal Calculate(string formula)
        {
            return Calculate<decimal>(formula);
        }

        private static void ExecuteOperation(ref Stack<Token> operands, ref Stack<OperationBase> operations)
        {
            OperationBase operation = operations.Pop();

            if (operation.IsUnary)
            {
                if (operands.Count < 1)
                {
                    throw new CalculateException(CalculateExceptionCode.MissingOperand);
                }

                Token operand = operands.Pop();

                operation.SetOperands(operand);
            }
            else
            {
                if (operands.Count < 2)
                {
                    throw new CalculateException(CalculateExceptionCode.MissingOperand);
                }

                Token rightOperand = operands.Pop();
                Token leftOperand = operands.Pop();

                operation.SetOperands(leftOperand, rightOperand);
            }

            Token result = operation.GetResult();

            operands.Push(result);
        }
    }
}
