using Calculator.Core.Enum;
using Calculator.Core.Parser;
using Xunit;

namespace Calculator.Core.Tests.Parser;

public class OperationParserTest
{
    private OperationParser parser = new();

    [Fact]
    public void TryParse_OperationAtBeginning_CorrectOperation()
    {
        Token? token;
        this.parser.TryParse("-3+5", out token, out _);

        this.AssertOperationTokenEqual(token, "-");
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
        this.AssertOperationTokenEqual(token, "+");

        this.parser.TryParse("*{1+3}", out token, out _);
        this.AssertOperationTokenEqual(token, "*");
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