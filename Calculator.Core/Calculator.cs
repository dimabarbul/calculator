using System;
using System.Collections.Generic;
using Calculator.Core.Enum;
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

            Stack<dynamic> operands = new Stack<dynamic>();
            Stack<OperationBase> operations = new Stack<OperationBase>();
            decimal result = 0;

            foreach (Token token in FormulaParser.GetTokens(formula))
            {
                if (token.Type == TokenType.Decimal || token.Type == TokenType.Bool)
                {
                    operands.Push(token.GetValue());
                }
                else if (token.Type == TokenType.Subformula)
                {
                    result = Calculator.Calculate(token.Text);

                    operands.Push(result);
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

            result = operands.Pop();

            return (TResult)Convert.ChangeType(result, typeof(TResult));
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

        private static void ExecuteOperation(ref Stack<dynamic> operands, ref Stack<OperationBase> operations)
        {
            OperationBase operation = operations.Pop();

            if (operation.IsUnary)
            {
                decimal operand = operands.Pop();

                operation.SetOperands(operand);
            }
            else
            {
                decimal rightOperand = operands.Pop();
                decimal leftOperand = operands.Pop();

                operation.SetOperands(leftOperand, rightOperand);
            }

            decimal result = operation.GetResult();

            operands.Push(result);
        }
    }
}
