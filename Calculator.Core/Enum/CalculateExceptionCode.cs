namespace Calculator.Core.Enum;

public enum CalculateExceptionCode
{
    NotSingleResult = 1,
    MissingOperand = 2,
    UnknownVariable = 3,
    InvalidResultType = 4,
    InvalidVariableType = 5,
    SubsequentOperators = 6,
}