﻿using Calculator.Core.Enums;
using Calculator.Core.Operands;

namespace Calculator.Core.Tokens;

public abstract class Function : Operation
{
    protected Function()
        : base(OperationPriority.Unary, 1)
    {
    }

    public override Token Execute(IList<Token> operands)
    {
        IReadOnlyList<Operand> functionOperands;

        if (operands[0] is ListOperand listOperand)
        {
            functionOperands = listOperand.Operands;
        }
        else
        {
            Operand[] convertedOperands = new Operand[operands.Count];
            for (int i = 0; i < operands.Count; i++)
            {
                convertedOperands[i] = (Operand)operands[i];
            }

            functionOperands = convertedOperands;
        }

        return this.ExecuteFunction(functionOperands);
    }

    public abstract string FunctionName { get; }

    protected abstract Token ExecuteFunction(IReadOnlyList<Operand> operands);
}