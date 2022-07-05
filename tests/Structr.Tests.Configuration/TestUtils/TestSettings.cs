using Structr.Configuration;

namespace Structr.Tests.Configuration.TestUtils
{
    public class TestSettings
    {
        public string FileName { get; set; }

        [Option(Alias = "SomeOwnerNameAlias")]
        public string OwnerName { get; set; }

        [Option(DefaultValue = "help.example.com")]
        public string HelpUrl { get; set; }

        [Option(EncryptionPassphrase = "abcdef")]
        public string ApiKey { get; set; }
    }
}
