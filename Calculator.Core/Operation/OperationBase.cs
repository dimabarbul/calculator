using Calculator.Core.Enum;

namespace Calculator.Core.Operation;

internal abstract class OperationBase
{
    protected Token leftOperand;
    protected Token rightOperand;

    public OperationPriority Priority { get; private set; }
    public bool IsUnary { get; private set; }

    public OperationBase(OperationPriority priority, bool isUnary)
    {
        this.Priority = priority;
        this.IsUnary = isUnary;
    }

    public void SetOperands(Token leftOperand, Token rightOperand = null)
    {
        this.leftOperand = leftOperand;

        if (rightOperand != null)
        {
            this.rightOperand = rightOperand;
        }
    }

    public abstract Token GetResult();
}