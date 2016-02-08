namespace Calculator.Core.Parser
{
    internal interface IParser
    {
        Token TryParse(string formula, ref int startIndex);
    }
}
