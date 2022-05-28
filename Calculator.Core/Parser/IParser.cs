namespace Calculator.Core.Parser;

public interface IParser
{
    int TryParse(string formula, out Token? token, int startIndex = 0);
}