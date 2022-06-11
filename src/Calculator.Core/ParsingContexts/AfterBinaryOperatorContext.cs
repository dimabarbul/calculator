namespace Calculator.Core.ParsingContexts;

internal record AfterBinaryOperatorContext() : ParsingContext(
    IsEndAllowed: false,
    IsBinaryOperatorAllowed: false,
    IsUnaryOperatorAllowed: true,
    IsFunctionAllowed: true,
    IsSubformulaAllowed: true,
    IsVariableAllowed: true,
    IsOperandAllowed: true);