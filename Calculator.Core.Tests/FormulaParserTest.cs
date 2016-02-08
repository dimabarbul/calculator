using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Calculator.Core.Tests
{
    [TestClass]
    public class FormulaParserTest
    {
        [TestMethod]
        public void GetTokens_EmptyString_ReturnsEmptyCollection()
        {
            Token[] tokens = FormulaParser.GetTokens(string.Empty).ToArray();

            Assert.AreEqual(0, tokens.Length);
        }

        [TestMethod]
        public void GetTokens_OneNumber_ReturnsNumberToken()
        {
            string formula = "1";
            Token[] tokens = FormulaParser.GetTokens(formula).ToArray();

            Assert.AreEqual(1, tokens.Length);
            this.AssertTokenEqual(tokens[0], formula, true);
        }

        [TestMethod]
        public void GetTokens_OneOperation_ReturnsOperationToken()
        {
            string[] formulas = new string[] { "-", "+", "*", "/" };
            Token[] tokens;

            foreach (string formula in formulas)
            {
                tokens = FormulaParser.GetTokens(formula).ToArray();

                Assert.AreEqual(1, tokens.Length);
                this.AssertTokenEqual(tokens[0], formula, false);
            }
        }

        [TestMethod]
        public void GetTokens_FormulaWithSpaces_TokensWithoutSpaces()
        {
            string[] formulas = new string[] { "  1   ", "-", " +", "*  " };
            Token[] tokens;

            foreach (string formula in formulas)
            {
                tokens = FormulaParser.GetTokens(formula).ToArray();

                Assert.AreEqual(1, tokens.Length);
                Assert.AreEqual(formula.Trim(), tokens[0].Value);
            }
        }

        [TestMethod]
        public void GetTokens_NumberWithLeadingPeriod_CorrectNumberToken()
        {
            Token[] tokens;

            tokens = FormulaParser.GetTokens(".34").ToArray();
            Assert.AreEqual(1, tokens.Length);
            this.AssertNumberToken(tokens[0], 0.34m);

            tokens = FormulaParser.GetTokens(".").ToArray();
            Assert.AreEqual(1, tokens.Length);
            this.AssertNumberToken(tokens[0], 0);
        }

        [TestMethod]
        public void GetTokens_OperationBeforeLeadingPeriod_SeparateTokens()
        {
            Token[] tokens = FormulaParser.GetTokens("+.").ToArray();

            Assert.AreEqual(2, tokens.Length);
            this.AssertTokenEqual(tokens[0], "+", false);
            this.AssertTokenEqual(tokens[1], ".", true);
        }

        private void AssertTokenEqual(Token token, string tokenValue, bool isNumber)
        {
            Assert.AreEqual(isNumber, token.IsNumber);
            Assert.AreEqual(tokenValue, token.Value);
        }

        private void AssertNumberToken(Token token, decimal value)
        {
            Assert.AreEqual(true, token.IsNumber);
            Assert.AreEqual(value, token.ToDecimal());
        }
    }
}
