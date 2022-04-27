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
            services.AddJsonConfiguration<AppSettings>(AppHelper.GetRootPath("appsettings.json"));
            services.AddXmlConfiguration<WebSettings>(AppHelper.GetRootPath("websettings.xml"));
            // For example, use AddDbConfiguration<AppSettings>(...) with custom DbSettingsProvider

            var serviceProvider = services.BuildServiceProvider();

            var app = serviceProvider.GetRequiredService<IApp>();

            return app.RunAsync();
        }
    }
}
