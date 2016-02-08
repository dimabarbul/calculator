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
            string formula = "some_unknown_function";
            this.parser.TryParse(formula, out token);

            this.AssertOperationTokenEqual(token, formula);
        }

        private void AssertOperationTokenEqual(Token token, string value)
        {
            Assert.AreEqual(TokenType.Operation, token.Type);
            Assert.AreEqual(value, token.GetValue());
        }
    }
}
