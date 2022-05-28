using System;
using Calculator.Core.Enum;

namespace Calculator.Core
{
    public class Token
    {
        public string Text { get; set; }
        public TokenType Type { get; set; }

        public Token(string text, TokenType type)
        {
            this.Text = text;
            this.Type = type;
        }

        public TResult GetValue<TResult>()
        {
            object value;

            switch (this.Type)
            {
                case TokenType.Decimal:
                    string fullNumber = (this.Text[0] == '.' ? "0" : string.Empty) + this.Text;

                    value = decimal.Parse(fullNumber);

                    break;
                case TokenType.Bool:
                    value = bool.Parse(this.Text);
                    break;
                default:
                    value = this.Text;
                    break;
            }

            return (TResult)Convert.ChangeType(value, typeof(TResult));
        }

        public dynamic GetValue()
        {
            dynamic value;

            switch (this.Type)
            {
                case TokenType.Decimal:
                    value = this.GetValue<decimal>();
                    break;
                case TokenType.Bool:
                    value = this.GetValue<bool>();
                    break;
                default:
                    value = this.GetValue<string>();
                    break;
            }

            return value;
        }
    }
}
