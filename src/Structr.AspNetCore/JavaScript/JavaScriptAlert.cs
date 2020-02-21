using System;

namespace Structr.AspNetCore.JavaScript
{
    public class JavaScriptAlert
    {
        public string Type { get; }
        public string Message { get; }

        public JavaScriptAlert(string type, string message)
        {
            if (string.IsNullOrEmpty(type))
                throw new ArgumentNullException(nameof(type));
            if (string.IsNullOrEmpty(message))
                throw new ArgumentNullException(nameof(message));

            Type = type;
            Message = message;
        }
    }
}
