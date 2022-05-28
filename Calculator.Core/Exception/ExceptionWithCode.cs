﻿namespace Calculator.Core.Exception;

public abstract class ExceptionWithCode : System.Exception
{
    public int Code { get; private set; }

    public ExceptionWithCode(int code, string message = null)
        : base(message)
    {
        this.Code = code;
    }
}