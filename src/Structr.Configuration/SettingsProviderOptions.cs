namespace Structr.Configuration
{
    /// <summary>
    /// Options for a settings provider <see cref="SettingsProvider{TSettings}"/>.
    /// </summary>
    public class SettingsProviderOptions
    {
        /// <summary>
        /// Determines whether settings should be cached.
        /// </summary>
        public bool Cache { get; set; }

        /// <summary>
        /// Initializes a new <see cref="SettingsProviderOptions"/> instance.
        /// </summary>
        public SettingsProviderOptions()
        {
            Cache = true;
        }
    }
}
