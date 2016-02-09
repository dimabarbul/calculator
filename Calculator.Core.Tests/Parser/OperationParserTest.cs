using Calculator.Core.Enum;
using Calculator.Core.Parser;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Calculator.Core.Tests.Parser
{
    [TestClass]
    public class OperationParserTest
    {
        private OperationParser parser = new OperationParser();

        [TestMethod]
        public void TryParse_OperationAtBeginning_CorrectOperation()
        {
            Token token;
            this.parser.TryParse("-3+5", out token);

            this.AssertOperationTokenEqual(token, "-");
        }

        [TestMethod]
        public void TryParse_OperationAtStartIndex_CorrectOperation()
        {
            Token token;
            this.parser.TryParse("1+2/3*4", out token, 3);

            this.AssertOperationTokenEqual(token, "/");
        }

        [TestMethod]
        public void TryParse_OperationNotAtBeginning_Null()
        {
            Token token;
            this.parser.TryParse("1-3+5", out token);

            Assert.IsNull(token);
        }

        [TestMethod]
        public void TryParse_OperationNotAtStartIndex_Null()
        {
            Token token;
            this.parser.TryParse("1-3+5", out token, 2);

            Assert.IsNull(token);
        }

        [TestMethod]
        public void TryParse_UnknownOperation_CorrectOperation()
        {
            Token token;
            this.parser.TryParse("some_unknown_function()", out token);

            this.AssertOperationTokenEqual(token, "some_unknown_function");
        }

        [TestMethod]
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

        private void AssertOperationTokenEqual(Token token, string value)
        {
            Assert.AreEqual(TokenType.Operation, token.Type);
            Assert.AreEqual(value, token.GetValue());
        }
    }
}
