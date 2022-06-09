namespace Structr.Configuration
{
    /// <summary>
    /// Options for a <see cref="SettingsProvider{TSettings}"/>.
    /// </summary>
    public class SettingsProviderOptions
    {
        /// <summary>
        /// Determines whether settings should be cached.
        /// </summary>
        /// <remarks>If settings source was changed than all the same will reload settings despite of this property value.</remarks>
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
