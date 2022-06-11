using Calculator.Core.Operands;
using Calculator.Core.ParsingContexts;
using Calculator.Core.Tokens;
using Calculator.Samples.ExtendingCalculator.Parsers;

namespace Calculator.Samples.ExtendingCalculator.Tests.Parsers;

public class StringOperandParserTest
{
    private readonly StringOperandParser parser = new();

    [Fact]
    public void TryParse_SimpleString_Parsed()
    {
        bool isParsed = this.parser.TryParse("\"test\"", ParsingContext.Initial, out Token? token, out int parsedLength);

        Assert.True(isParsed);
        Operand<string> stringOperand = Assert.IsType<Operand<string>>(token);
        Assert.Equal(6, parsedLength);
        Assert.Equal("test", stringOperand.Value);
    }

    [Fact]
    public void TryParse_EscapedQuotes_Parsed()
    {
        bool isParsed = this.parser.TryParse("\"test \\\" test\" something else", ParsingContext.Initial, out Token? token, out int parsedLength);

        Assert.True(isParsed);
        Operand<string> stringOperand = Assert.IsType<Operand<string>>(token);
        Assert.Equal(14, parsedLength);
        Assert.Equal("test \" test", stringOperand.Value);
    }

    [Fact]
    public void TryParse_EscapedBackslash_Parsed()
    {
        bool isParsed = this.parser.TryParse("\"test\\\\\" test\" something else", ParsingContext.Initial, out Token? token, out int parsedLength);

        Assert.True(isParsed);
        Operand<string> stringOperand = Assert.IsType<Operand<string>>(token);
        Assert.Equal(8, parsedLength);
        Assert.Equal("test\\", stringOperand.Value);
    }

    [Fact]
    public void TryParse_NoClosingQuote_NotParsed()
    {
        bool isParsed = this.parser.TryParse("\"test", ParsingContext.Initial, out Token? token, out _);

        Assert.False(isParsed);
        Assert.Null(token);
    }
}