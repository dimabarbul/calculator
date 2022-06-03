namespace Calculator.Core.Operands;

public class Variable : Token
{
    public Variable(string name)
    {
        this.Name = name;
    }

    public string Name { get; }
}