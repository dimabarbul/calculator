using System.Linq;
using Calculator.Core.Enum;
using Calculator.Core.Exception;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Calculator.Core.Tests
{
    [TestClass]
    public class FormulaTokenizerTest
    {
        [TestMethod]
        public void GetTokens_EmptyString_ReturnsEmptyCollection()
        {
            Token[] tokens = FormulaTokenizer.GetTokens(string.Empty).ToArray();

            Assert.AreEqual(0, tokens.Length);
        }

        [TestMethod]
        public void GetTokens_OneNumber_ReturnsNumberToken()
        {
            string formula = "1";
            Token[] tokens = FormulaTokenizer.GetTokens(formula).ToArray();

            Assert.AreEqual(1, tokens.Length);
            this.AssertNumberTokenEqual(tokens[0], 1);
        }

        [TestMethod]
        public void GetTokens_OneOperation_ReturnsOperationToken()
        {
            string[] formulas = new string[] { "-", "+", "*", "/" };
            Token[] tokens;

            foreach (string formula in formulas)
            {
                tokens = FormulaTokenizer.GetTokens(formula).ToArray();

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
                tokens = FormulaTokenizer.GetTokens(formula).ToArray();

                Assert.AreEqual(1, tokens.Length);
                Assert.AreEqual(formula.Trim(), tokens[0].Text);
            }
        }

        [TestMethod]
        public void GetTokens_NumberWithLeadingPeriod_CorrectNumberToken()
        {
            Token[] tokens;

            tokens = FormulaTokenizer.GetTokens(".34").ToArray();
            Assert.AreEqual(1, tokens.Length);
            this.AssertNumberTokenEqual(tokens[0], 0.34m);

            tokens = FormulaTokenizer.GetTokens(".").ToArray();
            Assert.AreEqual(1, tokens.Length);
            this.AssertNumberTokenEqual(tokens[0], 0);
        }

        [TestMethod]
        public void GetTokens_OperationBeforeLeadingPeriod_SeparateTokens()
        {
            Token[] tokens = FormulaTokenizer.GetTokens("+.").ToArray();

            Assert.AreEqual(2, tokens.Length);
            this.AssertSimpleTokenEqual(tokens[0], "+", false);
            this.AssertSimpleTokenEqual(tokens[1], ".", true);
        }

        [TestMethod]
        public void GetTokens_ExpressionInParenthesis_IsSubformula()
        {
            Token[] tokens = FormulaTokenizer.GetTokens("1 + (2 + 3)").ToArray();

            Assert.AreEqual(3, tokens.Length);
            Assert.AreEqual(TokenType.Subformula, tokens[2].Type);
            Assert.AreEqual("2+3", tokens[2].Text);
        }

        [TestMethod]
        public void GetTokens_UnaryPlus_Parsed()
        {
            Token[] tokens = FormulaTokenizer.GetTokens("+2").ToArray();

            Assert.AreEqual(2, tokens.Length);
            this.AssertSimpleTokenEqual(tokens[0], "+", false);
            this.AssertNumberTokenEqual(tokens[1], 2);
        }

        [TestMethod]
        public void GetTokens_UnaryMinus_Parsed()
        {
            Token[] tokens = FormulaTokenizer.GetTokens("-5").ToArray();

            Assert.AreEqual(2, tokens.Length);
            this.AssertSimpleTokenEqual(tokens[0], "-", false);
            this.AssertNumberTokenEqual(tokens[1], 5);
        }

        [TestMethod]
        public void GetTokens_TrueFalse_Parsed()
        {
            Token[] tokens = FormulaTokenizer.GetTokens("true").ToArray();

            Assert.AreEqual(1, tokens.Length);
            Assert.AreEqual("true", tokens[0].Text);
        }

        [TestMethod]
        public void GetTokens_UnknownOperation_DontThrowException()
        {
            FormulaTokenizer.GetTokens("1+-2").ToArray();
        }

        [TestMethod]
        [ExpectedExceptionWithCode(typeof(ParseException), (int)ParseExceptionCode.UnparsedToken)]
        public void GetTokens_UnmatchedClosingParenthesis_ThrowsException()
        {
            FormulaTokenizer.GetTokens(")-1)").ToArray();
        }

        private void AssertSimpleTokenEqual(Token token, string tokenValue, bool isNumber)
        {
            Assert.AreEqual(isNumber ? TokenType.Decimal : TokenType.Operation, token.Type);
            Assert.AreEqual(tokenValue, token.Text);
        }

        private void AssertNumberTokenEqual(Token token, decimal value)
        {
            Assert.AreEqual(TokenType.Decimal, token.Type);
            Assert.AreEqual(value, token.GetValue<decimal>());
        }
    }
}
