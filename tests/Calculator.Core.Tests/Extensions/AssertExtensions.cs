using System;
using Calculator.Core.Tokens;
using Xunit;

namespace Calculator.Core.Tests.Extensions;

internal static class AssertExtensions
{
    public static void TokenIs<TToken>(Token token, Action<TToken> predicate)
        where TToken : Token
    {
        TToken typedToken = Assert.IsAssignableFrom<TToken>(token);

        predicate(typedToken);
    }
}