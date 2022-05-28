using Calculator.Core.Enum;
using Calculator.Core.Exception;
using Calculator.Core.Operation;

namespace Calculator.Core;

public class Calculator
{
    private readonly FormulaTokenizer tokenizer;
    private readonly OperationFactory operationFactory;

    public Calculator(FormulaTokenizer tokenizer, OperationFactory operationFactory)
    {
        this.tokenizer = tokenizer;
        this.operationFactory = operationFactory;
    }

    public TResult Calculate<TResult>(string formula, Dictionary<string, object>? variables = null)
    {
        if (string.IsNullOrWhiteSpace(formula))
        {
            throw new ArgumentNullException();
        }

        Stack<Token> operands = new();
        Stack<OperationBase> operations = new();
        Token resultToken;
        Token? lastToken = null;

        foreach (Token token in this.tokenizer.GetTokens(formula))
        {
            switch (token.Type)
            {
                case TokenType.Decimal:
                case TokenType.Bool:
                    operands.Push(token);
                    break;
                case TokenType.Subformula:
                        resultToken = this.Calculate<Token>(token.Text);
                    operands.Push(resultToken);
                    break;
                case TokenType.Variable:
                    if (variables == null || !variables.ContainsKey(token.Text))
                    {
                        throw new CalculateException(CalculateExceptionCode.UnknownVariable);
                    }

                    TokenType variableType = this.tokenizer.DetectTokenType(variables[token.Text]);
                    if (!this.tokenizer.IsValueTokenType(variableType))
                    {
                        throw new CalculateException(CalculateExceptionCode.StringVariable);
                    }

                    Token variableToken = new(variables[token.Text].ToString(), variableType);

                    operands.Push(variableToken);
                    break;
                default:
                    OperationBase operation = this.operationFactory.Create(token, lastToken == null || lastToken.Type == TokenType.Operation);

                    while (operations.Count > 0)
                    {
                        OperationBase previousOperation = operations.Peek();

                        if (previousOperation.Priority >= operation.Priority)
                        {
                            this.ExecuteOperation(operands, operations);
                        }
                        else
                        {
                            break;
                        }
                    }

                    operations.Push(operation);
                    break;
            }

            lastToken = token;
        }

        while (operations.Count > 0)
        {
            this.ExecuteOperation(operands, operations);
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

    public decimal Calculate(string formula, Dictionary<string, object>? variables = null)
    {
        return this.Calculate<decimal>(formula, variables);
    }

    private void ExecuteOperation(Stack<Token> operands, Stack<OperationBase> operations)
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