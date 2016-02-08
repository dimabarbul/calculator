using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Calculator.Core
{
    internal static class FormulaParser
    {
        const string NumberPattern = @"\d+(\.\d*)?|\.\d*";
        const string OperationPattern = @"[^\d(.]+";

        public static IEnumerable<Token> GetTokens(string formula)
        {
            if (string.IsNullOrWhiteSpace(formula))
            {
                yield break;
            }

            string sanitizedFormula = (new Regex(@"\s*")).Replace(formula, string.Empty);

            int index = 0;
            Regex regex;
            bool isNumber;

            while (index < sanitizedFormula.Length)
            {
                if (char.IsDigit(sanitizedFormula[index]) || '.' == sanitizedFormula[index])
                {
                    regex = new Regex(NumberPattern);
                    isNumber = true;
                }
                else
                {
                    regex = new Regex(OperationPattern);
                    isNumber = false;
                }

                Match match = regex.Match(sanitizedFormula, index);
                index += match.Value.Length;

                yield return new Token(match.Value, isNumber);
            }
        }
    }
}
