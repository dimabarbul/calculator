using System.Linq;
using Calculator.Core.Enum;
using Calculator.Core.Exception;
using Xunit;

namespace Calculator.Core.Tests;

public class FormulaTokenizerTest
{
    [Fact]
    public void GetTokens_EmptyString_ReturnsEmptyCollection()
    {
        Token[] tokens = FormulaTokenizer.GetTokens(string.Empty).ToArray();

        Assert.Empty(tokens);
    }

    [Fact]
    public void GetTokens_OneNumber_ReturnsNumberToken()
    {
        string formula = "1";
        Token[] tokens = FormulaTokenizer.GetTokens(formula).ToArray();

        Assert.Single(tokens);
        this.AssertNumberTokenEqual(tokens[0], 1);
    }

    [Theory]
    [InlineData("-")]
    [InlineData("+")]
    [InlineData("*")]
    [InlineData("/")]
    public void GetTokens_OneOperation_ReturnsOperationToken(string formula)
    {
        Token[] tokens = FormulaTokenizer.GetTokens(formula).ToArray();

        Assert.Single(tokens);
        this.AssertTokenEqual(tokens[0], formula, TokenType.Operation);
    }

    [Theory]
    [InlineData("  1   ")]
    [InlineData("-")]
    [InlineData(" +")]
    [InlineData("*  ")]
    public void GetTokens_FormulaWithSpaces_TokensWithoutSpaces(string formula)
    {
        Token[] tokens = FormulaTokenizer.GetTokens(formula).ToArray();

        Assert.Single(tokens);
        Assert.Equal(formula.Trim(), tokens[0].Text);
    }

    [Theory]
    [InlineData(".34", 0.34)]
    [InlineData(".", 0)]
    public void GetTokens_NumberWithLeadingPeriod_CorrectNumberToken(string formula, decimal expectedValue)
    {
        Token[] tokens = FormulaTokenizer.GetTokens(formula).ToArray();
        Assert.Single(tokens);
        this.AssertNumberTokenEqual(tokens[0], expectedValue);
    }

    [Fact]
    public void GetTokens_OperationBeforeLeadingPeriod_SeparateTokens()
    {
        Token[] tokens = FormulaTokenizer.GetTokens("+.").ToArray();

        Assert.Equal(2, tokens.Length);
        this.AssertTokenEqual(tokens[0], "+", TokenType.Operation);
        this.AssertTokenEqual(tokens[1], ".", TokenType.Decimal);
    }

    [Fact]
    public void GetTokens_ExpressionInParenthesis_IsSubformula()
    {
        Token[] tokens = FormulaTokenizer.GetTokens("1 + (2 + 3)").ToArray();

        Assert.Equal(3, tokens.Length);
        this.AssertTokenEqual(tokens[2], "2+3", TokenType.Subformula);
    }

    [Fact]
    public void GetTokens_UnaryPlus_Parsed()
    {
        Token[] tokens = FormulaTokenizer.GetTokens("+2").ToArray();

        Assert.Equal(2, tokens.Length);
        this.AssertTokenEqual(tokens[0], "+", TokenType.Operation);
        this.AssertNumberTokenEqual(tokens[1], 2);
    }

    [Fact]
    public void GetTokens_UnaryMinus_Parsed()
    {
        Token[] tokens = FormulaTokenizer.GetTokens("-5").ToArray();

        Assert.Equal(2, tokens.Length);
        this.AssertTokenEqual(tokens[0], "-", TokenType.Operation);
        this.AssertNumberTokenEqual(tokens[1], 5);
    }

    [Theory]
    [InlineData("true")]
    [InlineData("false")]
    public void GetTokens_TrueFalse_Parsed(string formula)
    {
        Token[] tokens = FormulaTokenizer.GetTokens(formula).ToArray();

        Assert.Single(tokens);
        this.AssertTokenEqual(tokens[0], formula, TokenType.Bool);
    }

    [Fact]
    public void GetTokens_UnknownOperation_DontThrowException()
    {
        System.Exception? exception = Record.Exception(() => FormulaTokenizer.GetTokens("1+-2").ToArray());
            
        Assert.Null(exception);
    }

    [Theory]
    [InlineData("[1 + 2]", "1+2")]
    [InlineData("{ 5.2 }", "5.2")]
    [InlineData("<8 / 0>", "8/0")]
    public void GetTokens_DifferentParenthesis_ParsedAsSubformula(string formula, string subformula)
    {
        Token[] tokens = FormulaTokenizer.GetTokens(formula).ToArray();
        Assert.Single(tokens);
        this.AssertTokenEqual(tokens[0], subformula, TokenType.Subformula);
    }

    [Fact]
    public void GetTokens_Variable_Parsed()
    {
        Token[] tokens = FormulaTokenizer.GetTokens("test + test2").ToArray();

        Assert.Equal(3, tokens.Length);
        this.AssertTokenEqual(tokens[0], "test", TokenType.Variable);
        this.AssertTokenEqual(tokens[1], "+", TokenType.Operation);
        this.AssertTokenEqual(tokens[2], "test2", TokenType.Variable);
    }

    [Fact]
    public void GetTokens_VariableAndOperation_Parsed()
    {
        Token[] tokens = FormulaTokenizer.GetTokens("test() / test").ToArray();

        Assert.Equal(4, tokens.Length);
        this.AssertTokenEqual(tokens[0], "test", TokenType.Operation);
        this.AssertTokenEqual(tokens[1], "", TokenType.Subformula);
        this.AssertTokenEqual(tokens[2], "/", TokenType.Operation);
        this.AssertTokenEqual(tokens[3], "test", TokenType.Variable);
    }

    [Theory]
    [InlineData(2.3)]
    [InlineData("4")]
    public void DetectTokenType_Decimal_Correct(object value)
    {
        Assert.Equal(TokenType.Decimal, FormulaTokenizer.DetectTokenType(value));
    }

    [Theory]
    [InlineData(true)]
    [InlineData("true")]
    [InlineData(false)]
    [InlineData("false")]
    public void DetectTokenType_Bool_Correct(object value)
    {
        Assert.Equal(TokenType.Bool, FormulaTokenizer.DetectTokenType(value));
    }

    [Fact]
    public void DetectTokenType_Operation_Correct()
    {
        Assert.Equal(TokenType.Operation, FormulaTokenizer.DetectTokenType("+"));
    }

    [Fact]
    public void DetectTokenType_Subformula_Correct()
    {
        Assert.Equal(TokenType.Subformula, FormulaTokenizer.DetectTokenType("<1 - 3 + 5>"));
    }

    [Fact]
    public void DetectTokenType_Variable_Correct()
    {
        Assert.Equal(TokenType.Variable, FormulaTokenizer.DetectTokenType("test"));
    }

    [Fact]
    public void DetectTokenType_SeveralTokens_ThrowsException()
    {
        ParseException exception = Assert.Throws<ParseException>(() => FormulaTokenizer.DetectTokenType("1+2"));
        Assert.Equal((int)ParseExceptionCode.UnparsedToken, exception.Code);
    }

    [Fact]
    public void DetectTokenType_EmptyString_ThrowsException()
    {
        ParseException exception = Assert.Throws<ParseException>(() => FormulaTokenizer.DetectTokenType(string.Empty));
        Assert.Equal((int)ParseExceptionCode.UnparsedToken, exception.Code);
    }

    [Fact]
    public void GetTokens_UnmatchedClosingParenthesis_ThrowsException()
    {
        ParseException exception = Assert.Throws<ParseException>(() => FormulaTokenizer.GetTokens(")-1)").ToArray());
        Assert.Equal((int)ParseExceptionCode.UnparsedToken, exception.Code);
    }

    [Fact]
    public void GetTokens_NoClosingParenthesis_ThrowsException()
    {
        ParseException exception = Assert.Throws<ParseException>(() => FormulaTokenizer.GetTokens("2*(3-4").ToArray());
        Assert.Equal((int)ParseExceptionCode.UnparsedToken, exception.Code);
    }

    [Fact]
    public void GetTokens_ClosingParenthesisOfDifferentType_ThrowsException()
    {
        ParseException exception = Assert.Throws<ParseException>(() => FormulaTokenizer.GetTokens("2*(3-4>/4").ToArray());
        Assert.Equal((int)ParseExceptionCode.UnparsedToken, exception.Code);
    }

    private void AssertTokenEqual(Token token, string value, TokenType type)
    {
        Assert.Equal(type, token.Type);
        Assert.Equal(value, token.Text);
    }

    private void AssertNumberTokenEqual(Token token, decimal value)
    {
        Assert.Equal(TokenType.Decimal, token.Type);
        Assert.Equal(value, token.GetValue<decimal>());
    }
}