using System.Collections.Generic;
using System.Linq;
using Calculator.Core.Operation;
using Calculator.Core.Parser;
using Xunit;

namespace Calculator.Core.Tests.Parser;

public class FunctionParserTest
{
    private readonly FunctionParser parser;

    public FunctionParserTest(IEnumerable<IParser> parsers)
    {
        this.parser = (FunctionParser)parsers.Single(p => p is FunctionParser);
    }

    [Fact]
    public void TryParse_OperationAtBeginning_CorrectOperation()
    {
        Token? token;
        this.parser.TryParse("floor(1.2)", out token, out _);

        this.AssertFunctionTokenEqual(token, "floor");
    }

    [Fact]
    public void TryParse_OperationNotAtBeginning_Null()
    {
        Token? token;
        this.parser.TryParse("1-ceil(0.01)", out token, out _);

        Assert.Null(token);
    }

    [Fact]
    public void TryParse_UnknownOperation_Null()
    {
        Token? token;
        this.parser.TryParse("some_unknown_function()", out token, out _);

        Assert.Null(token);
    }

    [Fact]
    public void TryParse_StartsWithNumber_Null()
    {
        Token? token;
        this.parser.TryParse("2radians(180)", out token, out _);

        Assert.Null(token);
    }

    [Fact]
    public void TryParse_OperationFollowedByDifferentParenthesis_CorrectOperation()
    {
        Token? token;

        this.parser.TryParse("ceil(3)", out token, out _);
        this.AssertFunctionTokenEqual(token, "ceil");

        this.parser.TryParse("ceil<3>", out token, out _);
        this.AssertFunctionTokenEqual(token, "ceil");

        this.parser.TryParse("ceil[3]", out token, out _);
        this.AssertFunctionTokenEqual(token, "ceil");

        this.parser.TryParse("ceil{3}", out token, out _);
        this.AssertFunctionTokenEqual(token, "ceil");
    }

    [Fact]
    public void TryParse_EmptyString_Null()
    {
        Token? token;

        this.parser.TryParse(string.Empty, out token, out _);
        Assert.Null(token);
    }

    private void AssertFunctionTokenEqual(Token? token, string value)
    {
        Assert.NotNull(token);
        Function functionToken = Assert.IsAssignableFrom<Function>(token);
        Assert.Equal(value, functionToken.FunctionName);
    }
}