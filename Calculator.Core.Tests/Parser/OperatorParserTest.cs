using System.Collections.Generic;
using System.Linq;
using Calculator.Core.Operation;
using Calculator.Core.Parser;
using Xunit;

namespace Calculator.Core.Tests.Parser;

public class OperatorParserTest
{
    private readonly OperatorParser parser;

    public OperatorParserTest(IEnumerable<IParser> parsers)
    {
        this.parser = (OperatorParser)parsers.Single(p => p is OperatorParser);
    }

    [Fact]
    public void TryParse_OperationAtBeginning_CorrectOperation()
    {
        Token? token;
        this.parser.TryParse("-3+5", out token, out _);

        this.AssertOperatorTokenEqual(token, "-");
    }

    [Fact]
    public void TryParse_OperationNotAtBeginning_Null()
    {
        Token? token;
        this.parser.TryParse("1-3+5", out token, out _);

        Assert.Null(token);
    }

    [Fact]
    public void TryParse_OperationFollowedByDifferentParenthesis_CorrectOperation()
    {
        Token? token;

        this.parser.TryParse("+<3>", out token, out _);
        this.AssertOperatorTokenEqual(token, "+");

        this.parser.TryParse("*{1+3}", out token, out _);
        this.AssertOperatorTokenEqual(token, "*");
    }

    [Fact]
    public void TryParse_EmptyString_Null()
    {
        Token? token;

        this.parser.TryParse(string.Empty, out token, out _);
        Assert.Null(token);
    }

    private void AssertOperatorTokenEqual(Token? token, string value)
    {
        Assert.NotNull(token);
        Operator operatorToken = Assert.IsAssignableFrom<Operator>(token);
        Assert.Equal(value, operatorToken.Text);
    }
}