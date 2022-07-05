using System;

namespace Structr.Configuration
{
    /// <summary>
    /// Provides functionality for access to settings provider <see cref="SettingsProvider{TSettings}"/>.
    /// </summary>
    /// <typeparam name="TSettings">Settings class.</typeparam>
    public class ConfigurationOptions<TSettings> where TSettings : class, new()
    {
        /// <summary>
        /// Returns the settings provider <see cref="SettingsProvider{TSettings}"/>.
        /// </summary>
        public SettingsProvider<TSettings> Provider { get; }

        /// <summary>
        /// Initializes a new <see cref="ConfigurationOptions{TSettings}"/> instance.
        /// </summary>
        /// <param name="provider">The <see cref="SettingsProvider{TSettings}"/>.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="provider"/> is <see langword="null"/>.</exception>
        public ConfigurationOptions(SettingsProvider<TSettings> provider)
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            Provider = provider;
        }
    }
}
