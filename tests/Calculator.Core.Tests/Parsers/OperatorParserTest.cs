using System.Collections.Generic;
using System.Linq;
using Calculator.Core.Extensions;
using Calculator.Core.Operands;
using Calculator.Core.Parsers;
using Calculator.Core.ParsingContexts;
using Calculator.Core.Tokens;

namespace Calculator.Core.Tests.Parsers;

public class OperatorParserTest
{
    private readonly OperatorParser parser;
    private readonly ParsingContext context;

    public OperatorParserTest(IEnumerable<IParser> parsers)
    {
        this.parser = (OperatorParser)parsers.Single(p => p is OperatorParser);
        this.context = ParsingContext.Initial.SetNextToken(new ListOperand());
    }

    [Fact]
    public void TryParse_OperationAtBeginning_CorrectOperation()
    {
        bool isParsed = this.parser.TryParse("×3×5", this.context, out Token? token, out _);

        Assert.True(isParsed);
        this.AssertBinaryOperatorEqual(token, "×");
    }

    [Fact]
    public void TryParse_OperationNotAtBeginning_Null()
    {
        bool isParsed = this.parser.TryParse("1×3×5", this.context, out Token? token, out _);

        Assert.False(isParsed);
        Assert.Null(token);
    }

    [Theory]
    [InlineData("×<3>")]
    [InlineData("×{1×3}")]
    public void TryParse_OperationFollowedByDifferentParenthesis_CorrectOperation(string formula)
    {
        bool isParsed = this.parser.TryParse(formula, this.context, out Token? token, out _);

        Assert.True(isParsed);
        this.AssertBinaryOperatorEqual(token, "×");
    }

    [Fact]
    public void TryParse_EmptyString_Null()
    {
        bool isParsed = this.parser.TryParse(string.Empty, this.context, out Token? token, out _);

        Assert.False(isParsed);
        Assert.Null(token);
    }

    [Fact]
    public void TryParse_InInitialContext_UnaryOperator()
    {
        bool isParsed = this.parser.TryParse("×", ParsingContext.Initial, out Token? token, out _);

        Assert.True(isParsed);
        this.AssertUnaryOperatorEqual(token, "×");
    }

    private void AssertBinaryOperatorEqual(Token? token, string value)
    {
        Assert.NotNull(token);
        BinaryOperator operatorToken = Assert.IsAssignableFrom<BinaryOperator>(token);
        Assert.Equal(value, operatorToken.Text);
    }

    private void AssertUnaryOperatorEqual(Token? token, string value)
    {
        Assert.NotNull(token);
        UnaryOperator operatorToken = Assert.IsAssignableFrom<UnaryOperator>(token);
        Assert.Equal(value, operatorToken.Text);
    }

    public class MultiplyOperator : BinaryOperator
    {
        public override string Text => "×";

        public MultiplyOperator()
            : base(LowestPriority)
        {
        }

        public override Token Execute(IReadOnlyList<Token> operands)
        {
            operands.CheckValueType<decimal>();

            return new Operand<decimal>(((Operand<decimal>)operands[0]).Value * ((Operand<decimal>)operands[1]).Value);
        }
    }

    public class MultiplyUnaryOperator : UnaryOperator
    {
        public override string Text => "×";

        protected override Operand ExecuteUnaryOperator(Operand operand)
        {
            return operand;
        }
    }
}