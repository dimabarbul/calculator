﻿using Calculator.Extra.Enums;

namespace Calculator.Extra.Operators;

public class SubtractOperator : DecimalOperator
{
    public SubtractOperator()
        : base(OperationPriority.Subtract, 2, 1)
    {
    }

    public override string Text => "-";

    protected override decimal GetDecimalResult(params decimal[] operands)
    {
        if (operands.Length == 1)
        {
            return -operands[0];
        }

        return operands[0] - operands[1];
    }
}