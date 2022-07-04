using System;

namespace Structr.AspNetCore.Client.Alerts
{
    /// <summary>
    /// Represents alert of specified type with message.
    /// </summary>
    public class ClientAlert
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
        /// Initializes instance of <see cref="ClientAlert"/>.
        /// </summary>
        /// <param name="type">Type of alert.</param>
        /// <param name="message">Message sending with alert.</param>
        /// <exception cref="ArgumentNullException">When <paramref name="type"/> or <paramref name="message"/> is null or empty.</exception>
        public ClientAlert(string type, string message)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                throw new ArgumentNullException(nameof(type));
            }
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentNullException(nameof(message));
            }

            Type = type;
            Message = message;
        }
    }
}
