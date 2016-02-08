using Calculator.Core.Enum;

namespace Calculator.Core.Parser
{
    internal class SubformulaParser : IParser
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

            if ('(' != formula[startIndex])
            {
                return 0;
            }

            // Search corresponding closing parenthesis
            int openingParenthesisCount = 0;

            for (int index = startIndex + 1; index < formula.Length; index++)
            {
                if ('(' == formula[index])
                {
                    openingParenthesisCount++;
                }
                else if (')' == formula[index])
                {
                    if (0 == openingParenthesisCount)
                    {
                        string subformula = formula.Substring(startIndex + 1, index - startIndex - 1);

                        token = new Token(subformula, TokenType.Subformula);

                        break;
                    }
                    else
                    {
                        openingParenthesisCount--;
                    }
                }
            }

            return token.Text.Length + 2;
        }
    }
}
