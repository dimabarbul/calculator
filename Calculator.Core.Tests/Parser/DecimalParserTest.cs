using Calculator.Core.Enum;
using Calculator.Core.Parser;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Calculator.Core.Tests.Parser
{
    [TestClass]
    public class DecimalParserTest
    {
        private DecimalParser parser = new DecimalParser();

        [TestMethod]
        public void TryParse_NumberAtBeginning_CorrectNumber()
        {
            Token token;
            this.parser.TryParse("13.9 - 7", out token);

            this.AssertDecimalTokenEqual(token, 13.9m);
        }

        [TestMethod]
        public void TryParse_NumberAtStartIndex_CorrectNumber()
        {
            Token token;
            this.parser.TryParse("1+2", out token, 2);

            this.AssertDecimalTokenEqual(token, 2m);
        }

        [TestMethod]
        public void TryParse_NumberNotAtBeginning_Null()
        {
            Token token;
            this.parser.TryParse("-1", out token);

            Assert.IsNull(token);
        }

        [TestMethod]
        public void TryParse_NumberNotAtStartIndex_Null()
        {
            Token token;
            this.parser.TryParse("1+2", out token, 1);

            Assert.IsNull(token);
        }

        private void AssertDecimalTokenEqual(Token token, decimal value)
        {
            Assert.AreEqual(TokenType.Decimal, token.Type);
            Assert.AreEqual(value, token.GetValue());
        }
    }
}
