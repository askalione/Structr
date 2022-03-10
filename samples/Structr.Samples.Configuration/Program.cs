using Microsoft.Extensions.DependencyInjection;
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
            services.AddJsonConfiguration<AppSettings>(GetPath("appsettings.json"));
            services.AddXmlConfiguration<WebSettings>(GetPath("websettings.xml"));
            // For example, use AddDbConfiguration<AppSettings>(...) with custom DbSettingsProvider

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
