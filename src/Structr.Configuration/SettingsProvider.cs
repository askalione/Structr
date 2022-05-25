using Structr.Configuration.Internal;
using System;

namespace Structr.Configuration
{
    /// <summary>
    /// Provides functionality for access to a settings <see cref="TSettings"/>.
    /// </summary>
    /// <typeparam name="TSettings">Settings class.</typeparam>
    public abstract class SettingsProvider<TSettings> where TSettings : class, new()
    {
        private readonly SettingsProviderOptions _options;
        private TSettings _cache;

        /// <summary>
        /// Initializes a new <see cref="SettingsProvider{TSettings}"/> instance.
        /// </summary>
        /// <param name="options">The <see cref="SettingsProviderOptions"/>.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="options"/> is <see langword="null"/>.</exception>
        public SettingsProvider(SettingsProviderOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _options = options;
        }

        /// <summary>
        /// Returns current settings.
        /// </summary>
        public TSettings GetSettings()
        {
            if (_options.Cache == false || _cache == null)
            {
                _cache = LoadSettings();
            }
            else
            {
                var isModified = IsSettingsModified();
                if (isModified)
                {
                    var settings = LoadSettings();
                    Mapper.Map(settings, _cache);
                }
            }

            return _cache;
        }

        protected abstract TSettings LoadSettings();

        /// <summary>
        /// Set the settings.
        /// </summary>
        /// <param name="settings">Settings to set.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="settings"/> is <see langword="null"/>.</exception>
        public void SetSettings(TSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            UpdateSettings(settings);
        }

        protected abstract void UpdateSettings(TSettings settings);

        protected abstract bool IsSettingsModified();
    }
}
