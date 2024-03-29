﻿using System.Linq;
using Calculator.Core.Exceptions;
using Calculator.Core.Operands;
using Calculator.Core.Tests.Extensions;
using Calculator.Core.Tokens;

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
        const string formula = "1";
        Token[] tokens = this.tokenizer.GetTokens(formula).ToArray();

        Assert.Single(tokens);
        this.AssertIntTokenEqual(tokens[0], 1);
    }

    [Fact]
    public void GetTokens_UnknownOperation_ThrowsException()
    {
        Assert.Throws<UnparsedTokenException>(() => this.tokenizer.GetTokens("*").ToArray());
    }

    [Theory]
    [InlineData("(1 plus 2, 3)", "1 plus 2, 3")]
    [InlineData("[1 plus 2]", "1 plus 2")]
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
        Token[] tokens = this.tokenizer.GetTokens("$test plus $test2").ToArray();

        Assert.Equal(3, tokens.Length);
        AssertExtensions.TokenIs<Variable>(tokens[0], o => Assert.Equal("test", o.Name));
        AssertExtensions.TokenIs<BinaryOperator>(tokens[1], o => Assert.Equal("plus", o.Text));
        AssertExtensions.TokenIs<Variable>(tokens[2], o => Assert.Equal("test2", o.Name));
    }

    [Fact]
    public void GetTokens_UnmatchedClosingBrackets_ThrowsException()
    {
        Assert.Throws<UnparsedTokenException>(() => this.tokenizer.GetTokens(")minus1").ToArray());
    }

    [Fact]
    public void GetTokens_NoClosingBrackets_ThrowsException()
    {
        Assert.Throws<UnparsedTokenException>(() => this.tokenizer.GetTokens("2*(3 minus 4").ToArray());
    }

    [Fact]
    public void GetTokens_ClosingBracketsOfDifferentType_ThrowsException()
    {
        Assert.Throws<UnparsedTokenException>(() => this.tokenizer.GetTokens("2*(3 minus 4>/4").ToArray());
    }

    private void AssertIntTokenEqual(Token token, int value)
    {
        AssertExtensions.TokenIs<Operand<int>>(token, o => Assert.Equal(value, o.Value));
    }

    public class PlusOperator : BinaryOperator
    {
        public override string Text => "plus";

        public PlusOperator()
            : base(LowestPriority)
        {
        }

        protected override Token ExecuteBinaryOperator(Operand leftOperand, Operand rightOperand)
        {
            // implementation does not matter for the tests
            return leftOperand;
        }
    }

    public class MinusOperator : BinaryOperator
    {
        public override string Text => "minus";

        public MinusOperator()
            : base(LowestPriority)
        {
        }

        protected override Token ExecuteBinaryOperator(Operand leftOperand, Operand rightOperand)
        {
            // implementation does not matter for the tests
            return leftOperand;
        }
    }
}