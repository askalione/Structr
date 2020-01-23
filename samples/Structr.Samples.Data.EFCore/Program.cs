using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Structr.Samples.Data.EFCore.DataAccess;
using Structr.Abstractions.Extensions;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Structr.Data.EFCore;
using Microsoft.Extensions.Logging.Debug;

namespace Structr.Samples.Data.EFCore
{
    class Program
    {
        static Task Main(string[] args)
        {
            var services = new ServiceCollection();

            // Add sample app
            services.AddSample<App>();

            // EFCore
            services.AddDbContextPool<DataContext>(options =>
            {
                var connectionString = @"Data Source=(LocalDB)\mssqllocaldb;AttachDbFilename='|DataDirectory|\Application.mdf';Integrated Security=True"
                    .Replace("|DataDirectory|", new DirectoryInfo(
                        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)).GetParent(3).FullName);

                options
                    .UseLoggerProvider(new DebugLoggerProvider())
                    .UseSqlServer(connectionString);
            });

            var serviceProvider = services.BuildServiceProvider();

            var app = serviceProvider.GetRequiredService<IApp>();

            return app.RunAsync();
        }
    }
}
