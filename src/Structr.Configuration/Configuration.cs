using System;

namespace Structr.Configuration
{
    public class Configuration<TSettings> : IConfiguration<TSettings> where TSettings : class, new()
    {
        private readonly ConfigurationOptions<TSettings> _options;

        public TSettings Settings => _options.Provider.GetSettings();

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
