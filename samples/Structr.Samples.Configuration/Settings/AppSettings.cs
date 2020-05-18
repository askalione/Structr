using Structr.Configuration;

namespace Structr.Samples.Configuration.Settings
{
    public class AppSettings
    {
        [Setting(Alias = "app_name")]
        public string AppName { get; set; }
        public OAuthSettings OAuth { get; set; }
    }

    public class OAuthSettings
    {
        public string ClientId { get; set; }
        [Setting(EncryptionPassphrase = "&-AjC*pn_bDix+")]
        public string ClientSecret { get; set; }
    }
}
