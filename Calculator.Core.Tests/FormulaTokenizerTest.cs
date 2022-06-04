using System.Linq;
using Calculator.Core.Enums;
using Calculator.Core.Exceptions;
using Calculator.Core.Operands;
using Calculator.Core.Operations;
using Calculator.Core.Tests.Extensions;
using Xunit;

namespace Calculator.Core.Tests;

public class FormulaTokenizerTest
{
    private readonly FormulaTokenizer tokenizer;

    public FormulaTokenizerTest(FormulaTokenizer tokenizer)
    {
        this.tokenizer = tokenizer;
    }

    [Fact]
    public void GetTokens_EmptyString_ReturnsEmptyCollection()
    {
        Token[] tokens = this.tokenizer.GetTokens(string.Empty).ToArray();

        Assert.Empty(tokens);
    }

    [Fact]
    public void GetTokens_OneNumber_ReturnsNumberToken()
    {
        string formula = "1";
        Token[] tokens = this.tokenizer.GetTokens(formula).ToArray();

        Assert.Single(tokens);
        this.AssertDecimalTokenEqual(tokens[0], 1);
    }

    [Theory]
    [InlineData("-")]
    [InlineData("+")]
    [InlineData("*")]
    [InlineData("/")]
    public void GetTokens_OneOperation_ReturnsOperationToken(string formula)
    {
        Token[] tokens = this.tokenizer.GetTokens(formula).ToArray();

        Assert.Single(tokens);
        Operator operatorToken = Assert.IsAssignableFrom<Operator>(tokens[0]);
        Assert.Equal(formula, operatorToken.Text);
    }

    [Theory]
    [InlineData(".34", 0.34)]
    [InlineData(".", 0)]
    public void GetTokens_NumberWithLeadingPeriod_CorrectNumberToken(string formula, decimal expectedValue)
    {
        Token[] tokens = this.tokenizer.GetTokens(formula).ToArray();
        Assert.Single(tokens);
        this.AssertDecimalTokenEqual(tokens[0], expectedValue);
    }

    [Fact]
    public void GetTokens_OperationBeforeLeadingPeriod_SeparateTokens()
    {
        Token[] tokens = this.tokenizer.GetTokens("+.").ToArray();

        Assert.Equal(2, tokens.Length);
        AssertExtensions.TokenIs<Operator>(tokens[0], o => Assert.Equal("+", o.Text));
        AssertExtensions.TokenIs<Operand<decimal>>(tokens[1], o => Assert.Equal(0, o.Value));
    }

    [Fact]
    public void GetTokens_ExpressionInBrackets_IsSubformula()
    {
        Token[] tokens = this.tokenizer.GetTokens("1 + (2 + 3)").ToArray();

        Assert.Equal(3, tokens.Length);
        AssertExtensions.TokenIs<Subformula>(tokens[2], o => Assert.Equal("2 + 3", o.Text));
    }

    [Fact]
    public void GetTokens_UnaryPlus_Parsed()
    {
        Token[] tokens = this.tokenizer.GetTokens("+2").ToArray();

        Assert.Equal(2, tokens.Length);
        AssertExtensions.TokenIs<Operator>(tokens[0], o => Assert.Equal("+", o.Text));
        this.AssertDecimalTokenEqual(tokens[1], 2);
    }

    [Fact]
    public void GetTokens_UnaryMinus_Parsed()
    {
        Token[] tokens = this.tokenizer.GetTokens("-5").ToArray();

        Assert.Equal(2, tokens.Length);
        AssertExtensions.TokenIs<Operator>(tokens[0], o => Assert.Equal("-", o.Text));
        this.AssertDecimalTokenEqual(tokens[1], 5);
    }

    [Theory]
    [InlineData("true")]
    [InlineData("false")]
    public void GetTokens_TrueFalse_Parsed(string formula)
    {
        Token[] tokens = this.tokenizer.GetTokens(formula).ToArray();

        Assert.Single(tokens);
        AssertExtensions.TokenIs<Operand<bool>>(tokens[0], o => Assert.Equal(bool.Parse(formula), o.Value));
    }

    [Fact]
    public void GetTokens_UnknownOperation_DontThrowException()
    {
        System.Exception? exception = Record.Exception(() => this.tokenizer.GetTokens("1+-2").ToArray());

        Assert.Null(exception);
    }

    [Theory]
    [InlineData("[1 + 2]", "1 + 2")]
    [InlineData("{ 5.2 }", " 5.2 ")]
    [InlineData("<8 / 0>", "8 / 0")]
    public void GetTokens_DifferentBrackets_ParsedAsSubformula(string formula, string subformula)
    {
        Token[] tokens = this.tokenizer.GetTokens(formula).ToArray();
        Assert.Single(tokens);
        AssertExtensions.TokenIs<Subformula>(tokens[0], o => Assert.Equal(subformula, o.Text));
    }

    [Fact]
    public void GetTokens_Variable_Parsed()
    {
        Token[] tokens = this.tokenizer.GetTokens("$test + $test2").ToArray();

        Assert.Equal(3, tokens.Length);
        AssertExtensions.TokenIs<Variable>(tokens[0], o => Assert.Equal("test", o.Name));
        AssertExtensions.TokenIs<Operator>(tokens[1], o => Assert.Equal("+", o.Text));
        AssertExtensions.TokenIs<Variable>(tokens[2], o => Assert.Equal("test2", o.Name));
    }

    [Fact]
    public void GetTokens_VariableAndOperation_Parsed()
    {
        Token[] tokens = this.tokenizer.GetTokens("ceil(1) / $test").ToArray();

        Assert.Equal(4, tokens.Length);
        AssertExtensions.TokenIs<Function>(tokens[0], o => Assert.Equal("ceil", o.FunctionName));
        AssertExtensions.TokenIs<Subformula>(tokens[1], o => Assert.Equal("1", o.Text));
        AssertExtensions.TokenIs<Operator>(tokens[2], o => Assert.Equal("/", o.Text));
        AssertExtensions.TokenIs<Variable>(tokens[3], o => Assert.Equal("test", o.Name));
    }

    [Fact]
    public void GetTokens_UnmatchedClosingBrackets_ThrowsException()
    {
        ParseException exception = Assert.Throws<ParseException>(() => this.tokenizer.GetTokens(")-1)").ToArray());
        Assert.Equal((int)ParseExceptionCode.UnparsedToken, exception.Code);
    }

    [Fact]
    public void GetTokens_NoClosingBrackets_ThrowsException()
    {
        ParseException exception = Assert.Throws<ParseException>(() => this.tokenizer.GetTokens("2*(3-4").ToArray());
        Assert.Equal((int)ParseExceptionCode.UnparsedToken, exception.Code);
    }

    [Fact]
    public void GetTokens_ClosingBracketsOfDifferentType_ThrowsException()
    {
        ParseException exception = Assert.Throws<ParseException>(() => this.tokenizer.GetTokens("2*(3-4>/4").ToArray());
        Assert.Equal((int)ParseExceptionCode.UnparsedToken, exception.Code);
    }

    private void AssertDecimalTokenEqual(Token token, decimal value)
    {
        AssertExtensions.TokenIs<Operand<decimal>>(token, o => Assert.Equal(value, o.Value));
    }
}