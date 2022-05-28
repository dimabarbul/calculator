using Calculator.Core.Enum;
using Calculator.Core.Exception;
using Calculator.Core.Operation;

namespace Calculator.Core;

/// <summary>
/// Provides functionality to calculate formula values.
/// </summary>
public static class Calculator
{
    public static TResult Calculate<TResult>(string formula, Dictionary<string, object>? variables = null)
    {
        if (string.IsNullOrWhiteSpace(formula))
        {
            throw new ArgumentNullException();
        }

        Stack<Token> operands = new();
        Stack<OperationBase> operations = new();
        Token resultToken;

        foreach (Token token in FormulaTokenizer.GetTokens(formula))
        {
            switch (token.Type)
            {
                case TokenType.Decimal:
                case TokenType.Bool:
                    operands.Push(token);
                    break;
                case TokenType.Subformula:
                        resultToken = Calculator.Calculate<Token>(token.Text);
                    operands.Push(resultToken);
                    break;
                case TokenType.Variable:
                    if (variables == null || !variables.ContainsKey(token.Text))
                    {
                        throw new CalculateException(CalculateExceptionCode.UnknownVariable);
                    }

                    TokenType variableType = FormulaTokenizer.DetectTokenType(variables[token.Text]);
                    if (!FormulaTokenizer.IsValueTokenType(variableType))
                    {
                        throw new CalculateException(CalculateExceptionCode.StringVariable);
                    }

                    Token variableToken = new(variables[token.Text].ToString(), variableType);

                    operands.Push(variableToken);
                    break;
                default:
                    OperationBase operation = OperationFactory.Create(token, operands.Count == 0);

                    while (operations.Count > 0)
                    {
                        OperationBase previousOperation = operations.Peek();

                        if (previousOperation.Priority >= operation.Priority)
                        {
                            ExecuteOperation(operands, operations);
                        }
                        else
                        {
                            break;
                        }
                    }

                    operations.Push(operation);
                    break;
            }
        }

        while (operations.Count > 0)
        {
            ExecuteOperation(operands, operations);
        }

        if (operands.Count != 1)
        {
            throw new CalculateException(CalculateExceptionCode.NotSingleResult);
        }

        resultToken = operands.Pop();

        TResult result;

        if (typeof(Token) == typeof(TResult))
        {
            result = (TResult)Convert.ChangeType(resultToken, typeof(TResult));
        }
        else
        {
            result = resultToken.GetValue<TResult>();
        }

        return result;
    }

    public static decimal Calculate(string formula, Dictionary<string, object>? variables = null)
    {
        return Calculate<decimal>(formula, variables);
    }

    private static void ExecuteOperation(Stack<Token> operands, Stack<OperationBase> operations)
    {
        OperationBase operation = operations.Pop();

        if (operands.Count < operation.OperandsCount)
        {
            throw new CalculateException(CalculateExceptionCode.MissingOperand);
        }

        Token[] operandTokens = Enumerable.Range(0, operation.OperandsCount)
            .Select(_ => operands.Pop())
            .Reverse()
            .ToArray();

        Token result = operation.Perform(operandTokens);

        operands.Push(result);
    }
}