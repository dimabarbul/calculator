namespace Calculator.Core.ParsingContexts;

public record InitialContext() : ParsingContext(
    IsEndAllowed: true,
    IsBinaryOperatorAllowed: false,
    IsUnaryOperatorAllowed: true,
    IsFunctionAllowed: true,
    IsSubformulaAllowed: true,
    IsVariableAllowed: true,
    IsOperandAllowed: true);
