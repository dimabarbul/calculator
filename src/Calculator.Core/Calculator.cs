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

        MyStack<Token> operands = new();
        MyStack<Operation> operations = new();
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
                    if (isLastTokenOperator && operation is Operator)
                    {
                        throw new CalculateException(CalculateExceptionCode.SubsequentOperators, "Subsequent operators are not allowed");
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
            throw new CalculateException(CalculateExceptionCode.NotSingleResult, $"Expected formula {formula} to produce single result, but got {operands.Count}");
        }

        return operands.Pop();
    }

    private void ExecuteOperation(MyStack<Token> operands, MyStack<Operation> operations)
    {
        Operation operation = operations.Pop();

        if (operands.Count < operation.MinOperandsCount)
        {
            throw new CalculateException(CalculateExceptionCode.MissingOperand, $"Operation expected from {operation.MinOperandsCount} to {operation.MaxOperandsCount} operands, but there are only {operands.Count} left");
        }

        int count = Math.Min(operation.MaxOperandsCount, operands.Count);
        ArraySegment<Token> operandTokens = operands.Pop(count);

        Token result = operation.Execute(operandTokens);

        operands.Push(result);
    }
}