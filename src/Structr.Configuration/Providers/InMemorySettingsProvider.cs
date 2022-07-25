using System;

namespace Structr.Configuration.Providers
{
    /// <summary>
    /// Provides functionality for access to in memory settings <typeparamref name="TSettings"/>.
    /// </summary>
    public class InMemorySettingsProvider<TSettings> : SettingsProvider<TSettings>
        where TSettings : class, new()
    {
        private TSettings _settings;

        /// <summary>
        /// Initializes a new <see cref="InMemorySettingsProvider{TSettings}"/> instance.
        /// </summary>
        /// <param name="settings">The settings instance.</param>
        /// <param name="options">The options object to make additional configurations.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="options"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="settings"/> is <see langword="null"/>.</exception>
        public InMemorySettingsProvider(TSettings settings, SettingsProviderOptions options)
            : base(options)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            _settings = settings;
        }

        protected override bool IsSettingsModified()
            => true;

        protected override TSettings LoadSettings()
            => _settings;

        protected override void LogFirstAccess()
        { }

        protected override void UpdateSettings(TSettings settings)
            => _settings = settings;
    }
}
