using Calculator.Core.Enum;

namespace Calculator.Core.Exception
{
    internal class ParseException : ExceptionWithCode
    {
        public ParseException(ParseExceptionCode code)
            : base((int)code)
        {
        }
    }
}
