namespace Calculator.Core.Tokens;

public abstract class BinaryOperator : Operation
{
    protected BinaryOperator(int priority)
        : base(priority, 2)
    {
    }

    public override Token Execute(IReadOnlyList<Token> operands)
    {
        this.ValidateOperandsCount(operands);

        if (operands[0] is not Operand leftOperand || operands[1] is not Operand rightOperand)
        {
            throw new ArgumentException($"Expected all operands to be of type {typeof(Operand)}");
        }

        return this.ExecuteBinaryOperator(leftOperand, rightOperand);
    }

    protected abstract Token ExecuteBinaryOperator(Operand leftOperand, Operand rightOperand);

    public abstract string Text { get; }
}