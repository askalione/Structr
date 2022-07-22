using Structr.Configuration.Internal;
using System;

namespace Structr.Configuration
{
    /// <summary>
    /// Provides functionality for access to a settings <typeparamref name="TSettings"/>.
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
                LogFirstAccess();
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

        /// <summary>
        /// Load settings from its source.
        /// </summary>
        /// <returns>Loaded settings.</returns>
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

        /// <summary>
        /// Update settings in its source.
        /// </summary>
        /// <param name="settings">New settings</param>
        protected abstract void UpdateSettings(TSettings settings);

        /// <summary>
        /// Logs first access to source for further tracking of changes.
        /// </summary>
        protected abstract void LogFirstAccess();

        /// <summary>
        /// Determines whenever settings was modified and now differs from original values.
        /// </summary>
        /// <returns><see langword="true"/> if settings was changed, otherwise <see langword="false"/>.</returns>
        protected abstract bool IsSettingsModified();
    }
}
