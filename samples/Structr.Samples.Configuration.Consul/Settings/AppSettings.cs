using Structr.Configuration;

namespace Structr.Samples.Configuration.Consul.Settings
{
    public class AppSettings
    {
        [Option(Alias = "app_name")]
        public string AppName { get; set; } = default!;
        public OAuthSettings OAuth { get; set; } = default!;
    }

    public class OAuthSettings
    {
        public string ClientId { get; set; } = default!;
        [Option(EncryptionPassphrase = "&-AjC*pn_bDix+")]
        public string ClientSecret { get; set; } = default!;
    }
}
