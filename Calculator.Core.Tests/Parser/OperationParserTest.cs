using Calculator.Core.Enum;
using Calculator.Core.Parser;
using Xunit;

namespace Calculator.Core.Tests.Parser
{
    public class OperationParserTest
    {
        private OperationParser parser = new OperationParser();

        [Fact]
        public void TryParse_OperationAtBeginning_CorrectOperation()
        {
            Token token;
            this.parser.TryParse("-3+5", out token);

            this.AssertOperationTokenEqual(token, "-");
        }

        [Fact]
        public void TryParse_OperationAtStartIndex_CorrectOperation()
        {
            Token token;
            this.parser.TryParse("1+2/3*4", out token, 3);

            this.AssertOperationTokenEqual(token, "/");
        }

        [Fact]
        public void TryParse_OperationNotAtBeginning_Null()
        {
            Token token;
            this.parser.TryParse("1-3+5", out token);

            Assert.Null(token);
        }

        [Fact]
        public void TryParse_OperationNotAtStartIndex_Null()
        {
            Token token;
            this.parser.TryParse("1-3+5", out token, 2);

            Assert.Null(token);
        }

        [Fact]
        public void TryParse_UnknownOperation_CorrectOperation()
        {
            Token token;
            this.parser.TryParse("some_unknown_function()", out token);

            this.AssertOperationTokenEqual(token, "some_unknown_function");
        }

        [Fact]
        public void TryParse_OperationFollowedByDifferentParenthesis_CorrectOperation()
        {
            Token token;

            this.parser.TryParse("1+<3>", out token, 1);
            this.AssertOperationTokenEqual(token, "+");

            this.parser.TryParse("1+ceil[3]", out token, 2);
            this.AssertOperationTokenEqual(token, "ceil");

            this.parser.TryParse("1+(2*{1+3})", out token, 4);
            this.AssertOperationTokenEqual(token, "*");
        }

        [Fact]
        public void TryParse_EmptyString_Null()
        {
            Token token;

            this.parser.TryParse(string.Empty, out token);
            Assert.Null(token);
        }

        private void AssertOperationTokenEqual(Token token, string value)
        {
            Assert.Equal(TokenType.Operation, token.Type);
            Assert.Equal(value, token.GetValue());
        }
    }
}
