using Structr.Configuration;
using Structr.Samples.Configuration.Consul.Settings;

namespace Structr.Samples.Configuration.Consul
{
    public class App : IApp
    {
        private readonly IConfigurator<AppSettings> _configurator;
        private readonly IConfiguration<AppSettings> _configuration;
        private readonly ConfigurationOptions<AppSettings> _configurationOptions;

        public App(IConfigurator<AppSettings> configurator,
            IConfiguration<AppSettings> configuration,
            ConfigurationOptions<AppSettings> configurationOptions)
        {
            if (configurator == null)
            {
                throw new ArgumentNullException(nameof(configurator));
            }
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }
            if (configurationOptions == null)
            {
                throw new ArgumentNullException(nameof(configurationOptions));
            }

            _configurator = configurator;
            _configuration = configuration;
            _configurationOptions = configurationOptions;
        }

        public Task RunAsync()
        {
            // Seed
            _configurationOptions.Provider.SetSettings(new AppSettings());

            // Get settings
            var settings = _configuration.Settings;

            // Update settings
            _configurator.Configure(x =>
            {
                x.AppName = "Consul";
                x.OAuth = new OAuthSettings
                {
                    ClientId = "123",
                    ClientSecret = "secret"
                };
            });
            settings = _configuration.Settings;

            return Task.CompletedTask;
        }
    }
}
