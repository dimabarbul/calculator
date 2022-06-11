namespace Calculator.Core.ParsingContexts;

internal record AfterFunctionContext() : ParsingContext(
    IsEndAllowed: false,
    IsBinaryOperatorAllowed: false,
    IsUnaryOperatorAllowed: false,
    IsFunctionAllowed: false,
    IsSubformulaAllowed: true,
    IsVariableAllowed: false,
    IsOperandAllowed: false);