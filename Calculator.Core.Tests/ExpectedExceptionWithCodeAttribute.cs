using System;
using Calculator.Core.Exception;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Calculator.Core.Tests
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    internal class ExpectedExceptionWithCodeAttribute : ExpectedExceptionBaseAttribute
    {
        public Type ExceptionType { get; private set; }
        public int ExceptionCode { get; private set; }

        public ExpectedExceptionWithCodeAttribute(Type type, int code)
        {
            if (!type.IsSubclassOf(typeof(ExceptionWithCode)))
            {
                throw new ArgumentException("Expected exception type must be derived from ExceptionWithCode.");
            }

            this.ExceptionType = type;
            this.ExceptionCode = code;
        }

        protected override void Verify(System.Exception exception)
        {
            Assert.IsInstanceOfType(exception, this.ExceptionType);

            ExceptionWithCode exceptionWithCode = (ExceptionWithCode)exception;
            Assert.AreEqual(this.ExceptionCode, exceptionWithCode.Code);
        }
    }
}
