using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Calculator.Core.Parsers;

public class SubformulaParser : IParser
{
    private static readonly char[] OpeningBrackets = { '(', '[', '{', '<' };
    private static readonly char[] ClosingBrackets = { ')', ']', '}', '>' };

    public bool TryParse(ReadOnlySpan<char> formula, [NotNullWhen(true)] out Token? token, out int parsedLength)
    {
        token = null;
        parsedLength = default;

        if (formula.IsEmpty)
        {
            return false;
        }

        if (!this.IsOpeningParenthesis(formula[0]))
        {
            return false;
        }

        Stack<char> openingParenthesis = new();
        openingParenthesis.Push(formula[0]);

        for (int index = 1; index < formula.Length; index++)
        {
            if (this.IsOpeningParenthesis(formula[index]))
            {
                openingParenthesis.Push(formula[index]);
            }
            else if (this.IsClosingParenthesis(formula[index]))
            {
                char previousOpeningParenthesis = openingParenthesis.Pop();

                if (!this.AreBracketsOfSameType(previousOpeningParenthesis, formula[index]))
                {
                    break;
                }

                if (0 == openingParenthesis.Count)
                {
                    token = new Subformula(formula[1..index].ToString());

                    break;
                }
            }
        }

        if (token != null)
        {
            parsedLength = ((Subformula)token).Text.Length + 2;
        }

        return token != null;
    }

    private bool IsOpeningParenthesis(char c)
    {
        return ((IList)OpeningBrackets).Contains(c);
    }

    private bool IsClosingParenthesis(char c)
    {
        return ((IList)ClosingBrackets).Contains(c);
    }

    private bool AreBracketsOfSameType(char openingBrackets, char closingBrackets)
    {
        return Array.IndexOf(OpeningBrackets, openingBrackets) == Array.IndexOf(ClosingBrackets, closingBrackets);
    }
}