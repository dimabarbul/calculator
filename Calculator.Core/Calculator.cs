using Calculator.Core.Enums;
using Calculator.Core.Exceptions;
using Calculator.Core.Operands;
using Calculator.Core.Operations;

namespace Calculator.Core;

public class Calculator
{
    private readonly FormulaTokenizer tokenizer;

    public Calculator(FormulaTokenizer tokenizer)
    {
        this.tokenizer = tokenizer;
    }

    public TResult Calculate<TResult>(string formula, Dictionary<string, Operand>? variables = null)
    {
        Token lastOperand = this.Calculate(formula, variables);

        if (lastOperand is not Operand<TResult> resultToken)
        {
            throw new CalculateException(
                CalculateExceptionCode.InvalidResultType,
                $"Result is of type {lastOperand.GetType()}, but expected {typeof(Operand<TResult>)}");
        }

        return resultToken.Value;
    }

    public Token Calculate(string formula, Dictionary<string, Operand>? variables = null)
    {
        if (string.IsNullOrWhiteSpace(formula))
        {
            throw new ArgumentNullException(nameof(formula));
        }

        Stack<Token> operands = new();
        Stack<Operation> operations = new();
        bool isLastTokenOperator = false;

        foreach (Token token in this.tokenizer.GetTokens(formula))
        {
            switch (token)
            {
                case Subformula subformula:
                    operands.Push(this.Calculate(subformula.Text));
                    break;
                case Variable variable:
                    if (variables == null || !variables.ContainsKey(variable.Name))
                    {
                        throw new CalculateException(CalculateExceptionCode.UnknownVariable);
                    }

                    Type variableType = variables[variable.Name].GetType();
                    if (!variableType.IsGenericType || variableType.GetGenericTypeDefinition() != typeof(Operand<>))
                    {
                        throw new CalculateException(CalculateExceptionCode.InvalidVariableType);
                    }

                    operands.Push(variables[variable.Name]);
                    break;
                case Operand:
                    operands.Push(token);
                    break;
                case Operation operation:
                    if (isLastTokenOperator && operation is Operator)
                    {
                        throw new CalculateException(CalculateExceptionCode.SubsequentOperators);
                    }

                    while (operations.Count > 0)
                    {
                        Operation previousOperation = operations.Peek();

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

            isLastTokenOperator = token is Operator;
        }

        while (operations.Count > 0)
        {
            this.ExecuteOperation(operands, operations);
        }

        if (operands.Count != 1)
        {
            throw new CalculateException(CalculateExceptionCode.NotSingleResult);
        }

        return operands.Pop();
    }

    private void ExecuteOperation(Stack<Token> operands, Stack<Operation> operations)
    {
        Operation operation = operations.Pop();

        if (operands.Count < operation.MinOperandsCount)
        {
            throw new CalculateException(CalculateExceptionCode.MissingOperand);
        }

        Token[] operandTokens = Enumerable.Range(0, Math.Min(operation.MaxOperandsCount, operands.Count))
            .Select(_ => operands.Pop())
            .Reverse()
            .ToArray();

        Token result = operation.Execute(operandTokens);

        operands.Push(result);
    }
}