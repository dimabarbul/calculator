using System.Collections.Generic;
using System.Text.RegularExpressions;
using Calculator.Core.Operation;

namespace Calculator.Core
{
    /// <summary>
    /// Provides functionality to calculate formula values.
    /// </summary>
    public static class Calculator
    {
        /// <summary>
        /// Calculates provided formula.
        /// </summary>
        /// <param name="formula">Formula to calculate.</param>
        /// <returns>Calculation result.</returns>
        public static decimal Calculate(string formula)
        {
            if (string.IsNullOrWhiteSpace(formula))
            {
                return 0;
            }

            string sanitizedFormula = formula.Replace(" ", string.Empty);
            Stack<decimal> operands = new Stack<decimal>();
            Stack<OperationBase> operations = new Stack<OperationBase>();
            decimal result = 0;

            foreach (Token token in FormulaParser.GetTokens(sanitizedFormula))
            {
                if (token.IsNumber)
                {
                    operands.Push(token.ToDecimal());
                }
                else
                {
                    OperationBase operation = OperationFactory.Create(token);

                    operations.Push(operation);
                }
            }

            while (operations.Count > 0)
            {
                OperationBase operation = operations.Pop();
                decimal rightOperand = operands.Pop();
                decimal leftOperand = operands.Pop();

                operation.SetOperands(leftOperand, rightOperand);

                result = operation.GetResult();

                operands.Push(result);
            }

            result = operands.Pop();

            return result;
        }
    }
}
