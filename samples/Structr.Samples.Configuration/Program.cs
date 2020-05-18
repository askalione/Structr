using Microsoft.Extensions.DependencyInjection;
using Structr.Configuration;
using Structr.Samples.Configuration.Settings;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Structr.Samples.Configuration
{
    class Program
    {
        static Task Main(string[] args)
        {
            var services = new ServiceCollection();

            // Add sample app
            services.AddSample<App>();

            // Add Configuration
            services.AddConfiguration(options =>
            {
                options.Providers.AddJson<AppSettings>(GetPath("settings.json"));
            });

            var serviceProvider = services.BuildServiceProvider();

            var app = serviceProvider.GetRequiredService<IApp>();

            return app.RunAsync();
        }

        private static string GetPath(string filename)
        {
            var path = Path.Combine(new DirectoryInfo(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location))
                    .Parent
                    .Parent
                    .Parent
                    .FullName, filename);
            return path;
        }
    }
}
