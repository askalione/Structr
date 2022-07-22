using System;

namespace Structr.AspNetCore.Json
{
    /// <summary>
    /// Represents an error to be transferred to a client.
    /// </summary>
    public class JsonResponseError
    {
        /// <summary>
        /// Key corresponding to an error.
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// Message corresponding to an error.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Creates an instance of <see cref="JsonResponseError"/>.
        /// </summary>
        /// <param name="message">Message corresponding to error.</param>
        /// <exception cref="ArgumentNullException">When <paramref name="message"/> is null or white space.</exception>
        public JsonResponseError(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentNullException(nameof(message));
            }

            Message = message;
        }

        /// <inheritdoc cref="JsonResponseError"/>
        /// <param name="key">Key corresponding to error.</param>
        /// <param name="message">Message corresponding to error.</param>
        /// <exception cref="ArgumentNullException">When <paramref name="key"/> is null or white space.</exception>
        public JsonResponseError(string key, string message) : this(message)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            Key = key;
        }
    }
}