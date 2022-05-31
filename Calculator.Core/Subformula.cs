namespace Calculator.Core;

public class Subformula : Token
{
    public Subformula(string text)
    {
        this.Text = text;
    }

    public string Text { get; }
}