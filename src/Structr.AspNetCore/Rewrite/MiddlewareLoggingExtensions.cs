using Microsoft.Extensions.Logging;
using System;

namespace Structr.AspNetCore.Rewrite
{
    internal static class MiddlewareLoggingExtensions
    {
        private static readonly Action<ILogger, Exception> _redirectedToLowercase =
            LoggerMessage.Define(LogLevel.Information, new EventId(1, "RedirectedToLowercase"), "Request redirected to lowercase");

        public static void RedirectedToLowercase(this ILogger logger)
        {
            _redirectedToLowercase(logger, null);
        }
    }
}
