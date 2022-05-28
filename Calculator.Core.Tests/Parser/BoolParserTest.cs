using Calculator.Core.Enum;
using Calculator.Core.Parser;
using Xunit;

namespace Calculator.Core.Tests.Parser
{
    public class BoolParserTest
    {
        private BoolParser parser = new BoolParser();

        [Fact]
        public void TryParse_TrueAtBeginning_Correct()
        {
            Token token;
            this.parser.TryParse("true", out token);

            this.AssertBoolTokenEqual(token, true);
        }

        [Fact]
        public void TryParse_FalseAtBeginning_Correct()
        {
            Token token;
            this.parser.TryParse("false", out token);

            this.AssertBoolTokenEqual(token, false);
        }

        [Fact]
        public void TryParse_BoolAtStartIndex_Correct()
        {
            Token token;
            this.parser.TryParse("true||false", out token, 6);

            this.AssertBoolTokenEqual(token, false);
        }

        [Fact]
        public void TryParse_BoolNotAtStartIndex_Null()
        {
            Token token;
            this.parser.TryParse("true&&false", out token, 4);

            Assert.Null(token);
        }

        [Fact]
        public void TryParse_BoolNotAtBeginning_Null()
        {
            Token token;
            this.parser.TryParse("1||false", out token);

            Assert.Null(token);
        }

        [Fact]
        public void TryParse_EmptyString_Null()
        {
            Token token;

            this.parser.TryParse(string.Empty, out token);
            Assert.Null(token);
        }

        private void AssertBoolTokenEqual(Token token, bool value)
        {
            Assert.Equal(TokenType.Bool, token.Type);
            Assert.Equal(value, token.GetValue());
        }
    }
}
