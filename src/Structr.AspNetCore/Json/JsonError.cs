using System;

namespace Structr.AspNetCore.Json
{
    /// <summary>
    /// Represents an error to be transferred to a client.
    /// </summary>
    public class JsonError
    {
        /// <summary>
        /// Key corresponding to error.
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// Message corresponding to error.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Creates an instance of <see cref="JsonError"/>.
        /// </summary>
        /// <param name="message">Message corresponding to error.</param>
        /// <exception cref="ArgumentNullException">When <paramref name="message"/> is null or white space.</exception>
        public JsonError(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentNullException(nameof(message));
            }

            Message = message;
        }

        /// <inheritdoc cref="JsonError.JsonError"/>
        /// <param name="key">Key corresponding to error.</param>
        /// <exception cref="ArgumentNullException">When <paramref name="key"/> is null or white space.</exception>
        public JsonError(string key, string message) : this(message)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            Key = key;
        }
    }
}