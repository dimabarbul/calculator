using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calculator.Core.Enum;

namespace Calculator.Core.Parser
{
    internal class SubformulaParser : IParser
    {
        public Token TryParse(string formula, ref int startIndex)
        {
            if ('(' != formula[startIndex])
            {
                return null;
            }

            // Search corresponding closing parenthesis
            int openingParenthesisCount = 0;
            Token token = null;

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

            startIndex += token.Text.Length + 2;

            return token;
        }
    }
}
