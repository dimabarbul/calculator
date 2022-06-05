﻿using Calculator.Core.Enums;

namespace Calculator.Extra.Operators;

public class DivideOperator : DecimalOperator
{
    public DivideOperator()
        : base(OperationPriority.Divide, 2)
    {
    }

    public override string Text => "/";

    protected override decimal GetDecimalResult(params decimal[] operands)
    {
        return operands[0] / operands[1];
    }
}