﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Calculator.Core.Tests
{
    [TestClass]
    public class CalculatorTest
    {
        [TestMethod]
        public void Calculate_EmptyFormula_Zero()
        {
            Assert.AreEqual(0, Calculator.Calculate(string.Empty));
        }

        [TestMethod]
        public void Calculate_SimpleNumbersSum_Calculated()
        {
            Assert.AreEqual(3, Calculator.Calculate("1 + 2"));
            Assert.AreEqual(4, Calculator.Calculate("1.5 + 2.5"));
        }

        [TestMethod]
        public void Calculate_SimpleNumbersSubtraction_Calculated()
        {
            Assert.AreEqual(2, Calculator.Calculate("4 - 2"));
            Assert.AreEqual(2.3m, Calculator.Calculate("10.4 - 8.1"));
        }

        [TestMethod]
        public void Calculate_PeriodWithNumbers_MeansZeroPeriodNumber()
        {
            Assert.AreEqual(2, Calculator.Calculate(".5 + 1.5"));
            Assert.AreEqual(1, Calculator.Calculate("1 - ."));
        }

        [TestMethod]
        public void Calculate_SimpleNumbersMultiplication_Calculated()
        {
            Assert.AreEqual(6, Calculator.Calculate("3 * 2"));
            Assert.AreEqual(9.38m, Calculator.Calculate("1.4 * 6.7"));
        }
    }
}