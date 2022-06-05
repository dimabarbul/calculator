using System.Collections.Generic;
using System.Linq;
using Calculator.Core.Enums;
using Calculator.Core.Extensions;
using Calculator.Core.Operands;
using Calculator.Core.Parsers;
using Calculator.Core.Tokens;
using Xunit;

namespace Calculator.Core.Tests.Parsers;

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
        bool isParsed = this.parser.TryParse("×3×5", out Token? token, out _);

        Assert.True(isParsed);
        this.AssertOperatorTokenEqual(token, "×");
    }

    [Fact]
    public void TryParse_OperationNotAtBeginning_Null()
    {
        bool isParsed = this.parser.TryParse("1×3×5", out Token? token, out _);

        Assert.False(isParsed);
        Assert.Null(token);
    }

    [Theory]
    [InlineData("×<3>")]
    [InlineData("×{1×3}")]
    public void TryParse_OperationFollowedByDifferentParenthesis_CorrectOperation(string formula)
    {
        bool isParsed = this.parser.TryParse(formula, out Token? token, out _);

        Assert.True(isParsed);
        this.AssertOperatorTokenEqual(token, "×");
    }

    [Fact]
    public void TryParse_EmptyString_Null()
    {
        bool isParsed = this.parser.TryParse(string.Empty, out Token? token, out _);

        Assert.False(isParsed);
        Assert.Null(token);
    }

    private void AssertOperatorTokenEqual(Token? token, string value)
    {
        Assert.NotNull(token);
        Operator operatorToken = Assert.IsAssignableFrom<Operator>(token);
        Assert.Equal(value, operatorToken.Text);
    }

    public class MultiplyOperator : Operator
    {
        public override string Text => "×";

        public MultiplyOperator()
            : base(OperationPriority.Multiply, 2)
        {
        }

        public override Token Execute(IList<Token> operands)
        {
            operands.CheckValueType<decimal>();

            return new Operand<decimal>(((Operand<decimal>)operands[0]).Value * ((Operand<decimal>)operands[1]).Value);
        }
    }
}