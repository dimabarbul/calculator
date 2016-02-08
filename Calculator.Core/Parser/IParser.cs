namespace Calculator.Core.Parser
{
    internal interface IParser
    {
        int TryParse(string formula, out Token token, int startIndex = 0);
    }
}
