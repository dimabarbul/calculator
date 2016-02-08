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
                else if ('(' == sanitizedFormula[index])
                {
                    // Search corresponding closing parenthesis
                    int openingParenthesisCount = 0;
                    int startIndex = index;

                    for (index++; index < sanitizedFormula.Length; index++)
                    {
                        if ('(' == sanitizedFormula[index])
                        {
                            openingParenthesisCount++;
                        }
                        else if (')' == sanitizedFormula[index])
                        {
                            if (0 == openingParenthesisCount)
                            {
                                string subformula = sanitizedFormula.Substring(startIndex + 1, index - startIndex - 1);

                                yield return new Token(subformula, isSubformula: true);

                                index++;

                                break;
                            }
                            else
                            {
                                openingParenthesisCount--;
                            }
                        }
                    }

                    continue;
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
