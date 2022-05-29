using Calculator.Core.Enum;
using Calculator.Core.Parser;
using Xunit;

namespace Calculator.Core.Tests.Parser;

public class FunctionParserTest
{
    private FunctionParser parser = new();

    [Fact]
    public void TryParse_OperationAtBeginning_CorrectOperation()
    {
        Token? token;
        this.parser.TryParse("floor(1.2)", out token, out _);

        this.AssertOperationTokenEqual(token, "floor");
    }

    [Fact]
    public void TryParse_OperationNotAtBeginning_Null()
    {
        Token? token;
        this.parser.TryParse("1-ceil(0.01)", out token, out _);

        Assert.Null(token);
    }

    [Fact]
    public void TryParse_UnknownOperation_CorrectOperation()
    {
        Token? token;
        this.parser.TryParse("some_unknown_function()", out token, out _);

        this.AssertOperationTokenEqual(token, "some_unknown_function");
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
        this.AssertOperationTokenEqual(token, "ceil");

        this.parser.TryParse("ceil<3>", out token, out _);
        this.AssertOperationTokenEqual(token, "ceil");

        this.parser.TryParse("ceil[3]", out token, out _);
        this.AssertOperationTokenEqual(token, "ceil");

        this.parser.TryParse("ceil{3}", out token, out _);
        this.AssertOperationTokenEqual(token, "ceil");
    }

    [Fact]
    public void TryParse_EmptyString_Null()
    {
        Token? token;

        this.parser.TryParse(string.Empty, out token, out _);
        Assert.Null(token);
    }

    private void AssertOperationTokenEqual(Token? token, string value)
    {
        Assert.NotNull(token);
        Assert.Equal(TokenType.Operation, token.Type);
        Assert.Equal(value, token.GetValue());
    }
}