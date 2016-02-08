using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Calculator.Core.Tests
{
    [TestClass]
    public class TokenTest
    {
        [TestMethod]
        public void ToDecimal_Number_IsCorrectNumber()
        {
            Token token = new Token("2", true);

            Assert.AreEqual(true, token.IsNumber);
            Assert.AreEqual("2", token.Value);
            Assert.AreEqual(2m, token.ToDecimal());
        }

        [TestMethod]
        public void ToDecimal_Number_ValueNotChanged()
        {
            Token token = new Token(".3", true);

            Assert.AreEqual(true, token.IsNumber);
            Assert.AreEqual(".3", token.Value);
            Assert.AreEqual(0.3m, token.ToDecimal());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ToDecimal_NotNumberWithDigits_ThrowsException()
        {
            Token token = new Token("3", false);

            token.ToDecimal();
        }
    }
}
