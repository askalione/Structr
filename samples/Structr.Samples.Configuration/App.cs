using Microsoft.Extensions.DependencyInjection;
using Structr.Configuration;
using Structr.Samples.Configuration.Settings;
using System;
using System.Threading.Tasks;

namespace Structr.Samples.Configuration
{
    public class App : IApp
    {
        private readonly IServiceProvider _serviceProvider;

        public App(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            _serviceProvider = serviceProvider;
        }

        public Task RunAsync()
        {
            Run();
            return Task.CompletedTask;
        }

        public void Run()
        {
            // AppSettings
            RunAppSettings();

            // WebSettings
            RunWebSettings();
        }

        private void RunAppSettings()
        {
            var configuration = _serviceProvider.GetRequiredService<IConfiguration<AppSettings>>();
            var configurator = _serviceProvider.GetRequiredService<IConfigurator<AppSettings>>();

            // Get settings
            var settings = configuration.Settings;

            // Set settings
            configurator.Configure(settings =>
            {
                settings.AppName = "Structr";
            });
        }

        private void RunWebSettings()
        {
            var configuration = _serviceProvider.GetRequiredService<IConfiguration<WebSettings>>();
            var configurator = _serviceProvider.GetRequiredService<IConfigurator<WebSettings>>();

            // Get settings
            var settings = configuration.Settings;

            // Set settings
            configurator.Configure(settings =>
            {
                settings.CsvPath = @"D:\file.csv";
            });
        }
    }
}
