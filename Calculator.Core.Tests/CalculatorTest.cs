﻿using System;
using Calculator.Core.Enum;
using Calculator.Core.Exception;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Calculator.Core.Tests
{
    [TestClass]
    public class CalculatorTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Calculate_EmptyFormula_Zero()
        {
            Calculator.Calculate(string.Empty);
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

        [TestMethod]
        public void Calculate_SimpleNumbersDivision_Calculated()
        {
            Assert.AreEqual(1.5m, Calculator.Calculate("3 / 2"));
            Assert.AreEqual(7, Calculator.Calculate("14 / 2"));
        }

        [TestMethod]
        public void Calculate_OperationsWithDifferentPriorities_Calculated()
        {
            Assert.AreEqual(6, Calculator.Calculate("2 + 2 * 2"));
            Assert.AreEqual(0, Calculator.Calculate("3 / 3 - 1"));
            Assert.AreEqual(5, Calculator.Calculate("7 - 4 / 2"));
        }

        [TestMethod]
        [ExpectedException(typeof(DivideByZeroException))]
        public void Calculate_SimpleDivisionByZero_ThrowsException()
        {
            Calculator.Calculate("1 / 0");
        }

        [TestMethod]
        public void Calculate_SeveralOperationsWithSamePriority_LeftToRightOrder()
        {
            Assert.AreEqual(2m, Calculator.Calculate("2 - 2 + 2"));
            Assert.AreEqual(9m, Calculator.Calculate("3 / 1 * 3"));
            Assert.AreEqual(2m, Calculator.Calculate("2 + 2 - 2"));
            Assert.AreEqual(3m, Calculator.Calculate("3 * 1 / 3 * 3"));
        }

        [TestMethod]
        public void Calculate_Parenthesis_OverridesPriority()
        {
            Assert.AreEqual(8, Calculator.Calculate("(2 + 2) * 2"));
            Assert.AreEqual(0.5m, Calculator.Calculate("3 / (3 + 3)"));
        }

        [TestMethod]
        public void Calculate_UnaryPlusMinusInBeginning_Calculated()
        {
            Assert.AreEqual(-1, Calculator.Calculate("-1"));
            Assert.AreEqual(3, Calculator.Calculate("+3"));
            Assert.AreEqual(1, Calculator.Calculate("-1 + 2"));
        }

        [TestMethod]
        public void Calculate_UnaryPlusMinusInParenthesis_Calculated()
        {
            Assert.AreEqual(-3, Calculator.Calculate("2 + (-5)"));
            Assert.AreEqual(1, Calculator.Calculate("(-2) + (4 - 1)"));
            Assert.AreEqual(0.5m, Calculator.Calculate("1 / (+2)"));
            Assert.AreEqual(4, Calculator.Calculate("(+4) * 1"));
        }

        [TestMethod]
        public void Calculate_Floor_Calculated()
        {
            Assert.AreEqual(2m, Calculator.Calculate("floor(2.4)"));
            Assert.AreEqual(-1m, Calculator.Calculate("floor(-0.5)"));
            Assert.AreEqual(3, Calculator.Calculate("1 + floor(4 / 1.5)"));
        }

        [TestMethod]
        public void Calculate_Ceil_Calculated()
        {
            Assert.AreEqual(3m, Calculator.Calculate("ceil(2.4)"));
            Assert.AreEqual(0m, Calculator.Calculate("ceil(-0.5)"));
            Assert.AreEqual(-1, Calculator.Calculate("1 + ceil(-4 / 1.5)"));
        }

        [TestMethod]
        public void Calculate_OrOperation_Calculate()
        {
            Assert.AreEqual(false, Calculator.Calculate<bool>("false || false"));
            Assert.AreEqual(true, Calculator.Calculate<bool>("false || true"));
            Assert.AreEqual(true, Calculator.Calculate<bool>("true || false"));
            Assert.AreEqual(true, Calculator.Calculate<bool>("true || true"));
        }

        [TestMethod]
        public void Calculate_AndOperation_Calculate()
        {
            Assert.AreEqual(false, Calculator.Calculate<bool>("false && false"));
            Assert.AreEqual(false, Calculator.Calculate<bool>("false && true"));
            Assert.AreEqual(false, Calculator.Calculate<bool>("true && false"));
            Assert.AreEqual(true, Calculator.Calculate<bool>("true && true"));
        }

        [TestMethod]
        public void Calculate_NotOperation_Calculate()
        {
            Assert.AreEqual(false, Calculator.Calculate<bool>("!true"));
            Assert.AreEqual(true, Calculator.Calculate<bool>("!false"));
        }

        [TestMethod]
        public void Calculate_LogicalOperationsPriority_Calculate()
        {
            Assert.AreEqual(false, Calculator.Calculate<bool>("!true && false"));
            Assert.AreEqual(true, Calculator.Calculate<bool>("!false || true"));
        }

        [TestMethod]
        [ExpectedExceptionWithCode(typeof(ParseException), (int)ParseExceptionCode.UnparsedToken)]
        public void Calculate_UnmatchedClosingParenthesis_ThrowsParseException()
        {
            Calculator.Calculate(")+3");
        }

        [TestMethod]
        [ExpectedExceptionWithCode(typeof(CalculateException), (int)CalculateExceptionCode.UnknownOperation)]
        public void Calculate_UnknownOperation_ThrowsCalculateException()
        {
            Calculator.Calculate("1+-2");
        }

        [TestMethod]
        [ExpectedExceptionWithCode(typeof(CalculateException), (int)CalculateExceptionCode.NotSingleResult)]
        public void Calculate_SeveralResults_ThrowsCalculateException()
        {
            Calculator.Calculate("(1)(2)(3)");
        }
    }
}
