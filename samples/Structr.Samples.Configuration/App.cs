using Structr.Configuration;
using Structr.Samples.Configuration.Settings;
using Structr.Samples.IO;
using System;
using System.Threading.Tasks;

namespace Structr.Samples.Configuration
{
    public class App : IApp
    {
        private readonly IStringWriter _writer;
        private readonly IConfiguration<AppSettings> _configuration;
        private readonly IConfigurator<AppSettings> _configurator;

        public App(IStringWriter writer,
            IConfiguration<AppSettings> configuration,
            IConfigurator<AppSettings> configurator)
        {
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));
            if (configurator == null)
                throw new ArgumentNullException(nameof(configurator));

            _writer = writer;
            _configuration = configuration;
            _configurator = configurator;
        }

        public Task RunAsync()
        {
            Run();
            return Task.CompletedTask;
        }

        public void Run()
        {
            // Get settings
            var settings = _configuration.Settings;

            // Set settings
            _configurator.Configure(settings =>
            {
                settings.AppName = "Structr";
            });
        }
    }
}
