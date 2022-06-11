using Calculator.Core.Enums;
using Calculator.Core.Exceptions;
using Calculator.Core.Tokens;

namespace Calculator.Core.ParsingContexts;

public abstract record ParsingContext(
    bool IsEndAllowed,
    bool IsBinaryOperatorAllowed,
    bool IsUnaryOperatorAllowed,
    bool IsFunctionAllowed,
    bool IsSubformulaAllowed,
    bool IsVariableAllowed,
    bool IsOperandAllowed)
{
    public static readonly ParsingContext Initial = new InitialContext();

    private static readonly AfterBinaryOperatorContext BinaryOperatorContext = new();
    private static readonly AfterUnaryOperatorContext UnaryOperatorContext = new();
    private static readonly AfterFunctionContext FunctionContext = new();
    private static readonly AfterOperandContext OperandContext = new();
    private static readonly AfterSubformulaContext SubformulaContext = new();
    private static readonly AfterVariableContext VariableContext = new();

    public ParsingContext SetNextToken(Token token)
    {
        return token switch
        {
            BinaryOperator when this.IsBinaryOperatorAllowed => BinaryOperatorContext,
            UnaryOperator when this.IsUnaryOperatorAllowed => UnaryOperatorContext,
            Function when this.IsFunctionAllowed => FunctionContext,
            Operand when this.IsOperandAllowed => OperandContext,
            Subformula when this.IsSubformulaAllowed => SubformulaContext,
            Variable when this.IsVariableAllowed => VariableContext,
            _ => throw new ParseException(ParseExceptionCode.MisplacedToken,
                $"Token {token.GetType()} is not allowed in this context"),
        };
    }
}