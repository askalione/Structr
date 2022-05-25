using System;

namespace Structr.Configuration
{
    /// <summary>
    /// Atribute for configure a settings member
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class OptionAttribute : Attribute
    {
        /// <summary>
        /// Alias for a settings member.
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// Default value for a settings member.
        /// </summary>
        public object DefaultValue { get; set; }

        /// <summary>
        /// Password to encrypt a settings member value.
        /// </summary>
        public string EncryptionPassphrase { get; set; }
    }
}
