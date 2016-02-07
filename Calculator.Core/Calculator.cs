using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            string preparedFormula = formula.Replace(" ", string.Empty);

            string[] arguments = preparedFormula.Split('+');
            decimal[] argumentsAsDecimals = arguments.Select(a => decimal.Parse(a)).ToArray();

            decimal sum = argumentsAsDecimals.Sum();

            return sum;
        }
    }
}
