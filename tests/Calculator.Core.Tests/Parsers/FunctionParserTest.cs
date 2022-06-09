using System.Collections.Generic;
using System.Linq;
using Calculator.Core.Parsers;
using Calculator.Core.ParsingContexts;
using Calculator.Core.Tokens;

namespace Calculator.Core.Tests.Parsers;

public class FunctionParserTest
{
    private readonly FunctionParser parser;
    private readonly ParsingContext context = ParsingContext.Initial;

    public FunctionParserTest(IEnumerable<IParser> parsers)
    {
        this.parser = (FunctionParser)parsers.Single(p => p is FunctionParser);
    }

    [Fact]
    public void TryParse_OperationAtBeginning_CorrectOperation()
    {
        bool isParsed = this.parser.TryParse("test(1.2)", this.context, out Token? token, out _);

        Assert.True(isParsed);
        this.AssertFunctionTokenEqual(token, "test");
    }

    [Fact]
    public void TryParse_OperationNotAtBeginning_Null()
    {
        bool isParsed = this.parser.TryParse("1-test(0.01)", this.context, out Token? token, out _);

        Assert.False(isParsed);
        Assert.Null(token);
    }

    [Fact]
    public void TryParse_UnknownOperation_Null()
    {
        bool isParsed = this.parser.TryParse("some_unknown_function()", this.context, out Token? token, out _);

        Assert.False(isParsed);
        Assert.Null(token);
    }

    [Fact]
    public void TryParse_StartsWithNumber_Null()
    {
        bool isParsed = this.parser.TryParse("2radians(180)", this.context, out Token? token, out _);

        Assert.False(isParsed);
        Assert.Null(token);
    }

    [Theory]
    [InlineData("test(3)")]
    [InlineData("test<3>")]
    [InlineData("test[3]")]
    [InlineData("test{3}")]
    public void TryParse_OperationFollowedByDifferentParenthesis_CorrectOperation(string formula)
    {
        bool isParsed = this.parser.TryParse(formula, this.context, out Token? token, out _);
        Assert.True(isParsed);
        this.AssertFunctionTokenEqual(token, "test");
    }

    [Fact]
    public void TryParse_EmptyString_Null()
    {
        bool isParsed = this.parser.TryParse(string.Empty, this.context, out Token? token, out _);

        Assert.False(isParsed);
        Assert.Null(token);
    }

    private void AssertFunctionTokenEqual(Token? token, string value)
    {
        Assert.NotNull(token);
        Function functionToken = Assert.IsAssignableFrom<Function>(token);
        Assert.Equal(value, functionToken.FunctionName);
    }

    public class TestFunction : Function
    {
        public override string FunctionName => "test";

        protected override Token ExecuteFunction(IReadOnlyList<Operand> operands)
        {
            return operands[0];
        }
    }
}