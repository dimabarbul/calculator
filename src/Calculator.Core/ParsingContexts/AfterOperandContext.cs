﻿namespace Calculator.Core.ParsingContexts;

public record AfterOperandContext() : ParsingContext(
    IsEndAllowed: true,
    IsBinaryOperatorAllowed: true,
    IsUnaryOperatorAllowed: false,
    IsFunctionAllowed: false,
    IsSubformulaAllowed: false,
    IsVariableAllowed: false,
    IsOperandAllowed: false);