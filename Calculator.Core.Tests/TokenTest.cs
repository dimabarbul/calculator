using Calculator.Core.Enum;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Calculator.Core.Tests
{
    [TestClass]
    public class TokenTest
    {
        [TestMethod]
        public void GetValueDecimal_Number_CorrectValue()
        {
            Token token = new Token("2", TokenType.Decimal);

            Assert.AreEqual(2m, token.GetValue<decimal>());
        }

        [TestMethod]
        public void GetValueDecimal_DecimalWithLeadingPeriod_CorrectValue()
        {
            Token token = new Token(".3", TokenType.Decimal);

            Assert.AreEqual(0.3m, token.GetValue<decimal>());
        }

        [TestMethod]
        public void GetValue_WithTypeDecimal_Decimal()
        {
            Token token = new Token("4", TokenType.Decimal);

            dynamic value = token.GetValue();
            Assert.AreEqual(typeof(decimal), value.GetType());
            Assert.AreEqual(4m, value);
        }
    }
}
