using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Structr.Abstractions.Extensions;

namespace Structr.Samples.Abstractions
{
    class Program
    {
        static Task Main(string[] args)
        {
            var test = 2200.ToKiloFormatString();

            var services = new ServiceCollection();

            // Add sample app
            services.AddSample<App>();

            var serviceProvider = services.BuildServiceProvider();

            var app = serviceProvider.GetRequiredService<IApp>();

            return app.RunAsync();
        }
    }
}
