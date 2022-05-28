using Calculator.Core.Enum;
using Calculator.Core.Parser;
using Xunit;

namespace Calculator.Core.Tests.Parser
{
    public class VariableParserTest
    {
        private VariableParser parser = new VariableParser();

        [Fact]
        public void TryParse_VariableAtBeginning_Correct()
        {
            Token token;

            this.parser.TryParse("test", out token);
            this.AssertVariableTokenEqual(token, "test");
        }

        [Fact]
        public void TryParse_VariableAtStartIndex_Correct()
        {
            Token token;

            this.parser.TryParse("1-test", out token, 2);
            this.AssertVariableTokenEqual(token, "test");
        }

        [Fact]
        public void TryParse_VariableNotAtBeginning_Null()
        {
            Token token;

            this.parser.TryParse("2*test", out token);
            Assert.Null(token);
        }

        [Fact]
        public void TryParse_VariableNotAtStartIndex_Null()
        {
            Token token;

            this.parser.TryParse("ceil(1-test)", out token, 5);
            Assert.Null(token);
        }

        [Fact]
        public void TryParse_VariableFollowedByParenthesis_Null()
        {
            Token token;

            this.parser.TryParse("test()", out token);
            Assert.Null(token);
        }

        [Fact]
        public void TryParse_TrueFalse_Null()
        {
            Token token;

            this.parser.TryParse("true", out token);
            Assert.Null(token);

            this.parser.TryParse("false", out token);
            Assert.Null(token);
        }

        [Fact]
        public void TryParse_EmptyString_Null()
        {
            Token token;

            this.parser.TryParse(string.Empty, out token);
            Assert.Null(token);
        }

        private void AssertVariableTokenEqual(Token token, string value)
        {
            Assert.Equal(TokenType.Variable, token.Type);
            Assert.Equal(value, token.Text);
        }
    }
}
