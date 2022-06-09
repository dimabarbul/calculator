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

    public ParsingContext SetNextToken(Token token)
    {
        return token switch
        {
            BinaryOperator when this.IsBinaryOperatorAllowed => new AfterBinaryOperatorContext(),
            UnaryOperator when this.IsUnaryOperatorAllowed => new AfterUnaryOperatorContext(),
            Function when this.IsFunctionAllowed => new AfterFunctionContext(),
            Operand when this.IsOperandAllowed => new AfterOperandContext(),
            Subformula when this.IsSubformulaAllowed => new AfterSubformulaContext(),
            Variable when this.IsVariableAllowed => new AfterVariableContext(),
            _ => throw new ParseException(ParseExceptionCode.MisplacedToken,
                $"Token {token.GetType()} is not allowed in this context"),
        };
    }
}