using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Calculator.Core;

namespace Calculator.Core.Tests
{
    [TestClass]
    public class CalculatorTest
    {
        [TestMethod]
        public void Calculate_EmptyFormula_Zero()
        {
            string formula = string.Empty;
            decimal result = Calculator.Calculate(formula);

            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void Calculate_SimpleNumberSum_Calculated()
        {
            string formula = "1 + 2";
            decimal result = Calculator.Calculate(formula);

            Assert.AreEqual(3, result);
        }
    }
}
