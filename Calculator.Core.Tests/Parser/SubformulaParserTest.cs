﻿using Calculator.Core.Enum;
using Calculator.Core.Parser;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Calculator.Core.Tests.Parser
{
    [TestClass]
    public class SubformulaParserTest
    {
        private SubformulaParser parser = new SubformulaParser();

        [TestMethod]
        public void TryParse_SubformulaAtBeginning_Correct()
        {
            Token token;
            this.parser.TryParse("(1+2)-4", out token);

            this.AssertSubformulaTokenEqual(token, "1+2");
        }

        [TestMethod]
        public void TryParse_SubformulaAtStartIndex_Correct()
        {
            Token token;
            this.parser.TryParse("1+(2*4)", out token, 2);

            this.AssertSubformulaTokenEqual(token, "2*4");
        }

        [TestMethod]
        public void TryParse_SubformulaNotAtBeginning_Null()
        {
            Token token;
            this.parser.TryParse("1+(2*4)", out token);

            Assert.IsNull(token);
        }

        [TestMethod]
        public void TryParse_SubformulaNotAtStartIndex_Null()
        {
            Token token;
            this.parser.TryParse("1+(2*4)", out token, 4);

            Assert.IsNull(token);
        }

        private void AssertSubformulaTokenEqual(Token token, string value)
        {
            Assert.AreEqual(TokenType.Subformula, token.Type);
            Assert.AreEqual(value, token.GetValue());
        }
    }
}