using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Structr.Samples.Abstractions
{
    class Program
    {
        static Task Main(string[] args)
        {
            var services = new ServiceCollection();

            // Add sample app
            services.AddSample<App>();

            var serviceProvider = services.BuildServiceProvider();

            var app = serviceProvider.GetRequiredService<IApp>();

            return app.RunAsync();
        }
    }
}
