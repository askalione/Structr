using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Structr.Samples.Notices
{
    class Program
    {
        static Task Main(string[] args)
        {
            var services = new ServiceCollection();

            // Add sample app
            services.AddSample<App>();

            // Add notices
            services.AddNotices(typeof(Program).Assembly);

            var serviceProvider = services.BuildServiceProvider();

            var app = serviceProvider.GetRequiredService<IApp>();

            return app.RunAsync();
        }
    }
}
