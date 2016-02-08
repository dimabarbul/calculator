using System;
namespace Calculator.Core
{
    internal class Token
    {
        public string Value { get; set; }
        public bool IsNumber { get; private set; }

        public Token(string value, bool isNumber)
        {
            this.Value = value;
            this.IsNumber = isNumber;
        }

        public decimal ToDecimal()
        {
            if (!this.IsNumber)
            {
                throw new InvalidOperationException(string.Format(
                    @"Token ""{0}"" is not a number.", this.Value));
            }

            string fullNumber = (this.Value[0] == '.' ? "0" : string.Empty) + this.Value;

            return decimal.Parse(fullNumber);
        }
    }
}
