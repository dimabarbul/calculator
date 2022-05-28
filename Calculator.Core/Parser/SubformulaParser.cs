using System.Collections.Generic;
using System.Linq;
using Calculator.Core.Enum;

namespace Calculator.Core.Parser
{
    public class SubformulaParser : IParser
    {
        /// <summary>
        /// Tries to parse formula for subformula from specified position.
        /// Token is considered found only if it starts at startIndex.
        /// </summary>
        /// <param name="formula">Formula to parse.</param>
        /// <param name="token">Found token.</param>
        /// <param name="startIndex">Index to start searching at.</param>
        /// <returns>Number of characters token occupies.</returns>
        public int TryParse(string formula, out Token token, int startIndex = 0)
        {
            token = null;

            if (string.IsNullOrEmpty(formula))
            {
                return 0;
            }

            if (!this.IsOpeningParenthesis(formula[startIndex]))
            {
                return 0;
            }

            Stack<char> openingParenthesis = new Stack<char>();
            openingParenthesis.Push(formula[startIndex]);

            for (int index = startIndex + 1; index < formula.Length; index++)
            {
                if (this.IsOpeningParenthesis(formula[index]))
                {
                    openingParenthesis.Push(formula[index]);
                }
                else if (this.IsClosingParenthesis(formula[index]))
                {
                    char previousOpeningParenthesis = openingParenthesis.Pop();

                    if (!this.AreParenthesisOfSameType(previousOpeningParenthesis, formula[index]))
                    {
                        break;
                    }

                    if (0 == openingParenthesis.Count)
                    {
                        string subformula = formula.Substring(startIndex + 1, index - startIndex - 1);

                        token = new Token(subformula, TokenType.Subformula);

                        break;
                    }
                }
            }

            int result = 0;

            if (token != null)
            {
                result = token.Text.Length + 2;
            }

            return result;
        }

        private bool IsOpeningParenthesis(char c)
        {
            return new char[] { '(', '[', '{', '<' }.Contains(c);
        }

        private bool IsClosingParenthesis(char c)
        {
            return new char[] { ')', ']', '}', '>' }.Contains(c);
        }

        private bool AreParenthesisOfSameType(char openingParenthesis, char closingParenthesis)
        {
            return (
                (openingParenthesis == '(' && closingParenthesis == ')')
                || (openingParenthesis == '[' && closingParenthesis == ']')
                || (openingParenthesis == '{' && closingParenthesis == '}')
                || (openingParenthesis == '<' && closingParenthesis == '>')
            );
        }
    }
}
