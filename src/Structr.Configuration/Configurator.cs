using Microsoft.Extensions.DependencyInjection;
using System;

namespace Structr.Configuration
{
    /// <inheritdoc cref="IConfigurator{TSettings}"/>
    public class Configurator<TSettings> : IConfigurator<TSettings> where TSettings : class, new()
    {
        private readonly ConfigurationOptions<TSettings> _options;

        /// <summary>
        /// Initializes an instance of <see cref="Configurator{TSettings}"/>.
        /// </summary>
        /// <param name="options">The <see cref="ConfigurationOptions{TSettings}"/>.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="options"/> is <see langword="null"/>.</exception>
        public Configurator(ConfigurationOptions<TSettings> options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _options = options;
        }

        public void Configure(Action<TSettings> changes)
        {
            if (changes == null)
            {
                throw new ArgumentNullException(nameof(changes));
            }

            var provider = _options.Provider;
            var settings = provider.GetSettings();
            changes.Invoke(settings);
            provider.SetSettings(settings);
        }
    }
}
