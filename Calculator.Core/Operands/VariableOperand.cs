namespace Calculator.Core.Operands;

public class VariableOperand : Operand
{
    public VariableOperand(string name)
    {
        this.Name = name;
    }

    public string Name { get; }
}