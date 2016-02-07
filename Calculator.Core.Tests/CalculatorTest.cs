using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Calculator.Core;

namespace Calculator.Core.Tests
{
    [TestClass]
    public class CalculatorTest
    {
        [TestMethod]
        public void Calculate_SimpleSum_Calculated()
        {
            string formula = "1 + 2";
            int result = Calculator.Calculate(formula);

            Assert.AreEqual(3, result);
        }
    }
}
