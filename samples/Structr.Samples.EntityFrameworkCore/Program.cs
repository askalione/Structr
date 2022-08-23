using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Debug;
using Structr.Abstractions.Extensions;
using Structr.EntityFrameworkCore;
using Structr.Samples.EntityFrameworkCore.DataAccess;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Structr.Samples.EntityFrameworkCore
{
    class Program
    {
        static Task Main(string[] args)
        {
            var services = new ServiceCollection();

            // Add sample app
            services.AddSample<App>();

            // Infrastructure
            services.AddScoped<IPrincipal>((provider) =>
            {
                return new ClaimsPrincipal(new ClaimsIdentity(new List<Claim> {
                    new Claim(ClaimTypes.NameIdentifier, "1"),
                    new Claim(ClaimTypes.Name, "User-1")
                }));
            });

            // EntityFrameworkCore
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
