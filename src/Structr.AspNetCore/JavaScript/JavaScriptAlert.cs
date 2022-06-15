using System;

namespace Structr.AspNetCore.JavaScript
{
    /// <summary>
    /// Represents alert of specified type with message.
    /// </summary>
    public class JavaScriptAlert
    {
        /// <summary>
        /// Type of alert.
        /// </summary>
        public string Type { get; }

        /// <summary>
        /// Message sending with alert.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Initializes instance of <see cref="JavaScriptAlert"/>.
        /// </summary>
        /// <param name="type">Type of alert.</param>
        /// <param name="message">Message sending with alert.</param>
        /// <exception cref="ArgumentNullException">When <paramref name="type"/> or <paramref name="message"/> is null or empty.</exception>
        public JavaScriptAlert(string type, string message)
        {
            if (string.IsNullOrEmpty(type))
            {
                throw new ArgumentNullException(nameof(type));
            }
            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentNullException(nameof(message));
            }

            Type = type;
            Message = message;
        }
    }
}
