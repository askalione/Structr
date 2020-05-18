using System;

namespace Structr.Configuration
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class SettingAttribute : Attribute
    {
        public string Alias { get; set; }
        public object DefaultValue { get; set; }
        public string EncryptionPassphrase { get; set; }
    }
}
