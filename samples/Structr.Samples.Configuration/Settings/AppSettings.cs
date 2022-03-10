using Structr.Configuration;

namespace Structr.Samples.Configuration.Settings
{
    public class AppSettings
    {
        [Option(Alias = "app_name")]
        public string AppName { get; set; }
        public OAuthSettings OAuth { get; set; }
    }

    public class OAuthSettings
    {
        public string ClientId { get; set; }
        [Option(EncryptionPassphrase = "&-AjC*pn_bDix+")]
        public string ClientSecret { get; set; }
    }
}
