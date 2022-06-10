using Calculator.Collections;
using Calculator.Core.Enums;
using Calculator.Core.Exceptions;
using Calculator.Core.Operands;
using Calculator.Core.Tokens;

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
        Token result = this.Calculate(formula, variables);

        if (result is NullToken)
        {
            throw new ArgumentNullException(nameof(formula));
        }

        if (result is not Operand<TResult> resultToken)
        {
            throw new CalculateException(
                CalculateExceptionCode.InvalidResultType,
                $"Result is of type {result.GetType()}, but expected {typeof(Operand<TResult>)}");
        }

        return resultToken.Value;
    }

    public Token Calculate(string formula, Dictionary<string, Operand>? variables = null)
    {
        if (string.IsNullOrWhiteSpace(formula))
        {
            return NullToken.Instance;
        }

        // TODO: extract some class?
        MyStack<Token> operands = new();
        MyStack<Operation> operations = new();

        foreach (Token token in this.tokenizer.GetTokens(formula))
        {
            switch (token)
            {
                case Subformula subformula:
                    operands.Push(this.Calculate(subformula.Text, variables));
                    break;
                case Variable variable:
                    if (variables == null || !variables.ContainsKey(variable.Name))
                    {
                        throw new CalculateException(CalculateExceptionCode.UnknownVariable, $"Unknown variable {variable.Name}");
                    }

                    Type variableType = variables[variable.Name].GetType();
                    if (!variableType.IsGenericType || variableType.GetGenericTypeDefinition() != typeof(Operand<>))
                    {
                        throw new CalculateException(CalculateExceptionCode.InvalidVariableType, $"Invalid type of variable {variable.Name}");
                    }

                    operands.Push(variables[variable.Name]);
                    break;
                case Operand:
                    operands.Push(token);
                    break;
                case Operation operation:
                    if (operation.IsLeftToRight)
                    {
                        while (operations.Count > 0)
                        {
                            Operation previousOperation = operations.Peek();

                            if (previousOperation.IsLeftToRight)
                            {
                                break;
                            }

                            this.ExecuteOperation(operands, operations);
                        }

                        while (operations.Count > 0)
                        {
                            Operation previousOperation = operations.Peek();

                            if (previousOperation.Priority <= operation.Priority &&
                                (previousOperation.Priority != operation.Priority || !previousOperation.IsLeftToRight))
                            {
                                break;
                            }

                            this.ExecuteOperation(operands, operations);
                        }
                    }

                    operations.Push(operation);
                    break;
            }
        }

        while (operations.Count > 0)
        {
            this.ExecuteOperation(operands, operations);
        }

        if (operands.Count != 1)
        {
            throw new CalculateException(CalculateExceptionCode.NotSingleResult, $"Expected formula {formula} to produce single result, but got {operands.Count}");
        }

        return operands.Pop();
    }

    private void ExecuteOperation(MyStack<Token> operands, MyStack<Operation> operations)
    {
        Operation operation = operations.Pop();

        if (operands.Count < operation.OperandsCount)
        {
            throw new CalculateException(CalculateExceptionCode.MissingOperand, $"Operation expected {operation.OperandsCount} operands, but there are only {operands.Count} left");
        }

        int count = Math.Min(operation.OperandsCount, operands.Count);
        IReadOnlyList<Token> operandTokens = operands.Pop(count);

        Token result = operation.Execute(operandTokens);

        operands.Push(result);
    }
}