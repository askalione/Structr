using System;

namespace Structr.Configuration
{
    /// <inheritdoc cref="IConfiguration{TSettings}"/>
    public class Configuration<TSettings> : IConfiguration<TSettings> where TSettings : class, new()
    {
        private readonly ConfigurationOptions<TSettings> _options;

        public TSettings Settings => _options.Provider.GetSettings();

        /// <summary>
        /// Initializes an instance of <see cref="Configuration{TSettings}"/>.
        /// </summary>
        /// <param name="options">The <see cref="ConfigurationOptions{TSettings}"/>.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="options"/> is <see langword="null"/>.</exception>
        public Configuration(ConfigurationOptions<TSettings> options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _options = options;
        }
    }
}
