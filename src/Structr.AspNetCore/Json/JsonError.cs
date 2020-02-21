using System;

namespace Structr.AspNetCore.Json
{
    public class JsonError
    {
        public string Key { get; }
        public string Message { get; }

        public JsonError(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentNullException(nameof(message));

            Message = message;
        }

        public JsonError(string key, string message) : this(message)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            Key = key;
        }
    }
}