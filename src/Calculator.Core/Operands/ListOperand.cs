namespace Calculator.Core.Operands;

public class ListOperand : Operand
{
    private readonly List<Operand> operands;

    public IReadOnlyList<Operand> Operands => this.operands.AsReadOnly();

    public ListOperand(params Operand[] operands)
    {
        this.operands = new List<Operand>(operands);
    }

    public void Add(Operand operand)
    {
        this.operands.Add(operand);
    }
}