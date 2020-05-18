namespace Structr.Configuration
{
    public class ConfigurationOptions
    {
        public SettingsProviderDictionary Providers { get; set; }
        public bool Cache { get; set; }

        public ConfigurationOptions()
        {
            Providers = new SettingsProviderDictionary();
            Cache = true;
        }
    }
}
