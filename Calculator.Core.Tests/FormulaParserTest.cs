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
            this.AssertSimpleTokenEqual(tokens[0], formula, true);
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
                this.AssertSimpleTokenEqual(tokens[0], formula, false);
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
            this.AssertNumberTokenEqual(tokens[0], 0.34m);

            tokens = FormulaParser.GetTokens(".").ToArray();
            Assert.AreEqual(1, tokens.Length);
            this.AssertNumberTokenEqual(tokens[0], 0);
        }

        [TestMethod]
        public void GetTokens_OperationBeforeLeadingPeriod_SeparateTokens()
        {
            Token[] tokens = FormulaParser.GetTokens("+.").ToArray();

            Assert.AreEqual(2, tokens.Length);
            this.AssertSimpleTokenEqual(tokens[0], "+", false);
            this.AssertSimpleTokenEqual(tokens[1], ".", true);
        }

        [TestMethod]
        public void GetTokens_ExpressionInParenthesis_IsSubformula()
        {
            Token[] tokens = FormulaParser.GetTokens("1 + (2 + 3)").ToArray();

            Assert.AreEqual(3, tokens.Length);
            Assert.AreEqual(true, tokens[2].IsSubformula);
            Assert.AreEqual("2+3", tokens[2].Value);
        }

        private void AssertSimpleTokenEqual(Token token, string tokenValue, bool isNumber)
        {
            Assert.AreEqual(isNumber, token.IsNumber);
            Assert.AreEqual(tokenValue, token.Value);
            Assert.AreEqual(false, token.IsSubformula);
        }

        private void AssertNumberTokenEqual(Token token, decimal value)
        {
            Assert.AreEqual(true, token.IsNumber);
            Assert.AreEqual(value, token.ToDecimal());
            Assert.AreEqual(false, token.IsSubformula);
        }
    }
}
