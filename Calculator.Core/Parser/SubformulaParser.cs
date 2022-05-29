using System.Diagnostics.CodeAnalysis;
using Calculator.Core.Enum;

namespace Calculator.Core.Parser;

public class SubformulaParser : IParser
{
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

                if (!this.AreParenthesisOfSameType(previousOpeningParenthesis, formula[index]))
                {
                    break;
                }

                if (0 == openingParenthesis.Count)
                {
                    string subformula = formula[1..index].ToString();

                    token = new Token(subformula, TokenType.Subformula);

                    break;
                }
            }
        }

        if (token != null)
        {
            parsedLength = token.Text.Length + 2;
        }

        return token != null;
    }

    private bool IsOpeningParenthesis(char c)
    {
        return new[] { '(', '[', '{', '<' }.Contains(c);
    }

    private bool IsClosingParenthesis(char c)
    {
        return new[] { ')', ']', '}', '>' }.Contains(c);
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