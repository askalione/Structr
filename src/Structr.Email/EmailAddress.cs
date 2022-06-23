using System;

namespace Structr.Email
{
    /// <summary>
    /// Represents an object containing data about an email address.
    /// </summary>
    public class EmailAddress
    {
        /// <summary>
        /// Email address name.
        /// </summary>
        public string? Name { get; }

        /// <summary>
        /// Email address.
        /// </summary>
        public string Address { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailAddress"/> class with the <paramref name="address"/> and <paramref name="name"/>.
        /// </summary>
        /// <param name="address">The email address.</param>
        /// <param name="name">The email address name.</param>
        /// /// <exception cref="ArgumentNullException">If <paramref name="address"/> is <see langword="null"/> or empty.</exception>
        public EmailAddress(string address, string? name)
            : this(address)
        {
            Name = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailAddress"/> class with the <paramref name="address"/>.
        /// </summary>
        /// <param name="address">The email address.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="address"/> is <see langword="null"/> or empty.</exception>
        public EmailAddress(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
            {
                throw new ArgumentNullException(nameof(address));
            }

            Address = address;
        }

        /// <summary>
        /// Returns a string that represents the current <see cref="EmailAddress"/> in the format "{Address}" or "{Name}<{Address}>".
        /// </summary>
        public override string ToString()
        {
            return string.IsNullOrWhiteSpace(Name)
                ? Address
                : $"{Name} <{Address}>";
        }
    }
}
