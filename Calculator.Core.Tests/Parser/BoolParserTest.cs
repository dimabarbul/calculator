using Calculator.Core.Enum;
using Calculator.Core.Parser;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Calculator.Core.Tests.Parser
{
    [TestClass]
    public class BoolParserTest
    {
        private BoolParser parser = new BoolParser();

        [TestMethod]
        public void TryParse_TrueAtBeginning_Correct()
        {
            Token token;
            this.parser.TryParse("true", out token);

            this.AssertBoolTokenEqual(token, true);
        }

        [TestMethod]
        public void TryParse_FalseAtBeginning_Correct()
        {
            Token token;
            this.parser.TryParse("false", out token);

            this.AssertBoolTokenEqual(token, false);
        }

        [TestMethod]
        public void TryParse_BoolAtStartIndex_Correct()
        {
            Token token;
            this.parser.TryParse("true||false", out token, 6);

            this.AssertBoolTokenEqual(token, false);
        }

        [TestMethod]
        public void TryParse_BoolNotAtStartIndex_Null()
        {
            Token token;
            this.parser.TryParse("true&&false", out token, 4);

            Assert.IsNull(token);
        }

        [TestMethod]
        public void TryParse_BoolNotAtBeginning_Null()
        {
            Token token;
            this.parser.TryParse("1||false", out token);

            Assert.IsNull(token);
        }

        [TestMethod]
        public void TryParse_EmptyString_Null()
        {
            Token token;

            this.parser.TryParse(string.Empty, out token);
            Assert.IsNull(token);
        }

        private void AssertBoolTokenEqual(Token token, bool value)
        {
            Assert.AreEqual(TokenType.Bool, token.Type);
            Assert.AreEqual(value, token.GetValue());
        }
    }
}
