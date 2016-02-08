using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Calculator.Core
{
    internal static class FormulaParser
    {
        const string NumberPattern = @"\d+(\.\d*)?|\.\d*";
        const string OperationPattern = @"[\+-]";

        public static IEnumerable<Token> GetTokens(string formula)
        {
            if (string.IsNullOrWhiteSpace(formula))
            {
                yield break;
            }

            int index = 0;
            Regex regex;
            bool isNumber;

            while (index < formula.Length)
            {
                if (char.IsDigit(formula[index]) || '.' == formula[index])
                {
                    regex = new Regex(NumberPattern);
                    isNumber = true;
                }
                else
                {
                    regex = new Regex(OperationPattern);
                    isNumber = false;
                }

                Match match = regex.Match(formula, index);
                index += match.Value.Length;

                yield return new Token(match.Value, isNumber);
            }
        }
    }
}
