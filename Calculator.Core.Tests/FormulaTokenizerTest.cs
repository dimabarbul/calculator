﻿using System.Linq;
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
                this.AssertTokenEqual(tokens[0], formula, TokenType.Operation);
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
            this.AssertTokenEqual(tokens[0], "+", TokenType.Operation);
            this.AssertTokenEqual(tokens[1], ".", TokenType.Decimal);
        }

        [TestMethod]
        public void GetTokens_ExpressionInParenthesis_IsSubformula()
        {
            Token[] tokens = FormulaTokenizer.GetTokens("1 + (2 + 3)").ToArray();

            Assert.AreEqual(3, tokens.Length);
            this.AssertTokenEqual(tokens[2], "2+3", TokenType.Subformula);
        }

        [TestMethod]
        public void GetTokens_UnaryPlus_Parsed()
        {
            Token[] tokens = FormulaTokenizer.GetTokens("+2").ToArray();

            Assert.AreEqual(2, tokens.Length);
            this.AssertTokenEqual(tokens[0], "+", TokenType.Operation);
            this.AssertNumberTokenEqual(tokens[1], 2);
        }

        [TestMethod]
        public void GetTokens_UnaryMinus_Parsed()
        {
            Token[] tokens = FormulaTokenizer.GetTokens("-5").ToArray();

            Assert.AreEqual(2, tokens.Length);
            this.AssertTokenEqual(tokens[0], "-", TokenType.Operation);
            this.AssertNumberTokenEqual(tokens[1], 5);
        }

        [TestMethod]
        public void GetTokens_TrueFalse_Parsed()
        {
            Token[] tokens = FormulaTokenizer.GetTokens("true").ToArray();

            Assert.AreEqual(1, tokens.Length);
            this.AssertTokenEqual(tokens[0], "true", TokenType.Bool);
            
            tokens = FormulaTokenizer.GetTokens("false").ToArray();

            Assert.AreEqual(1, tokens.Length);
            this.AssertTokenEqual(tokens[0], "false", TokenType.Bool);
        }

        [TestMethod]
        public void GetTokens_UnknownOperation_DontThrowException()
        {
            FormulaTokenizer.GetTokens("1+-2").ToArray();
        }

        [TestMethod]
        public void GetTokens_DifferentParenthesis_ParsedAsSubformula()
        {
            Token[] tokens;

            tokens = FormulaTokenizer.GetTokens("[1 + 2]").ToArray();
            Assert.AreEqual(1, tokens.Length);
            this.AssertTokenEqual(tokens[0], "1+2", TokenType.Subformula);

            tokens = FormulaTokenizer.GetTokens("{ 5.2 }").ToArray();
            Assert.AreEqual(1, tokens.Length);
            this.AssertTokenEqual(tokens[0], "5.2", TokenType.Subformula);

            tokens = FormulaTokenizer.GetTokens("<8 / 0>").ToArray();
            Assert.AreEqual(1, tokens.Length);
            this.AssertTokenEqual(tokens[0], "8/0", TokenType.Subformula);
        }

        [TestMethod]
        public void GetTokens_Variable_Parsed()
        {
            Token[] tokens = FormulaTokenizer.GetTokens("test + test2").ToArray();

            Assert.AreEqual(3, tokens.Length);
            this.AssertTokenEqual(tokens[0], "test", TokenType.Variable);
            this.AssertTokenEqual(tokens[1], "+", TokenType.Operation);
            this.AssertTokenEqual(tokens[2], "test2", TokenType.Variable);
        }

        [TestMethod]
        public void GetTokens_VariableAndOperation_Parsed()
        {
            Token[] tokens = FormulaTokenizer.GetTokens("test() / test").ToArray();

            Assert.AreEqual(4, tokens.Length);
            this.AssertTokenEqual(tokens[0], "test", TokenType.Operation);
            this.AssertTokenEqual(tokens[1], "", TokenType.Subformula);
            this.AssertTokenEqual(tokens[2], "/", TokenType.Operation);
            this.AssertTokenEqual(tokens[3], "test", TokenType.Variable);
        }

        [TestMethod]
        public void DetectTokenType_Decimal_Correct()
        {
            Assert.AreEqual(TokenType.Decimal, FormulaTokenizer.DetectTokenType(2.3));
            Assert.AreEqual(TokenType.Decimal, FormulaTokenizer.DetectTokenType("4"));
        }

        [TestMethod]
        public void DetectTokenType_Bool_Correct()
        {
            Assert.AreEqual(TokenType.Bool, FormulaTokenizer.DetectTokenType(true));
            Assert.AreEqual(TokenType.Bool, FormulaTokenizer.DetectTokenType("true"));
            Assert.AreEqual(TokenType.Bool, FormulaTokenizer.DetectTokenType(false));
            Assert.AreEqual(TokenType.Bool, FormulaTokenizer.DetectTokenType("false"));
        }

        [TestMethod]
        public void DetectTokenType_Operation_Correct()
        {
            Assert.AreEqual(TokenType.Operation, FormulaTokenizer.DetectTokenType("+"));
        }

        [TestMethod]
        public void DetectTokenType_Subformula_Correct()
        {
            Assert.AreEqual(TokenType.Subformula, FormulaTokenizer.DetectTokenType("<1 - 3 + 5>"));
        }

        [TestMethod]
        public void DetectTokenType_Variable_Correct()
        {
            Assert.AreEqual(TokenType.Variable, FormulaTokenizer.DetectTokenType("test"));
        }

        [TestMethod]
        [ExpectedExceptionWithCode(typeof(ParseException), (int)ParseExceptionCode.UnparsedToken)]
        public void DetectTokenType_SeveralTokens_ThrowsException()
        {
            FormulaTokenizer.DetectTokenType("1+2");
        }

        [TestMethod]
        [ExpectedExceptionWithCode(typeof(ParseException), (int)ParseExceptionCode.UnparsedToken)]
        public void DetectTokenType_EmptyString_ThrowsException()
        {
            FormulaTokenizer.DetectTokenType(string.Empty);
        }

        [TestMethod]
        [ExpectedExceptionWithCode(typeof(ParseException), (int)ParseExceptionCode.UnparsedToken)]
        public void GetTokens_UnmatchedClosingParenthesis_ThrowsException()
        {
            FormulaTokenizer.GetTokens(")-1)").ToArray();
        }

        [TestMethod]
        [ExpectedExceptionWithCode(typeof(ParseException), (int)ParseExceptionCode.UnparsedToken)]
        public void GetTokens_NoClosingParenthesis_ThrowsException()
        {
            FormulaTokenizer.GetTokens("2*(3-4").ToArray();
        }

        [TestMethod]
        [ExpectedExceptionWithCode(typeof(ParseException), (int)ParseExceptionCode.UnparsedToken)]
        public void GetTokens_ClosingParenthesisOfDifferentType_ThrowsException()
        {
            FormulaTokenizer.GetTokens("2*(3-4>/4").ToArray();
        }

        private void AssertTokenEqual(Token token, string value, TokenType type)
        {
            Assert.AreEqual(type, token.Type);
            Assert.AreEqual(value, token.Text);
        }

        private void AssertNumberTokenEqual(Token token, decimal value)
        {
            Assert.AreEqual(TokenType.Decimal, token.Type);
            Assert.AreEqual(value, token.GetValue<decimal>());
        }
    }
}
