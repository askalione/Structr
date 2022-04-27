using Microsoft.Extensions.DependencyInjection;
using Structr.Samples.Configuration.Settings;
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
            services.AddConfiguration()
                .AddJson<AppSettings>(AppHelper.GetRootPath("appsettings.json"))
                .AddXml<WebSettings>(AppHelper.GetRootPath("websettings.xml"));
            // Use `.AddProvider<AppSettings>(...)` with custom DbSettingsProvider, for example

            var serviceProvider = services.BuildServiceProvider();

            var app = serviceProvider.GetRequiredService<IApp>();

            return app.RunAsync();
        }
    }
}
