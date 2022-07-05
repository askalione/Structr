using Microsoft.Extensions.DependencyInjection;
using Structr.Abstractions.Extensions;
using Structr.Abstractions.Providers.Timestamp;
using Structr.Samples.EntityFramework.DataAccess;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Claims;
using System.Security.Principal;

namespace Structr.Samples.EntityFramework
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();

            // Add sample app
            services.AddSample<App>();

            // Infrastructure
            services.AddTimestampProvider<LocalTimestampProvider>();
            services.AddScoped<IPrincipal>((provider) =>
            {
                return new ClaimsPrincipal(new ClaimsIdentity(new List<Claim> {
                    new Claim(ClaimTypes.NameIdentifier, "1"),
                    new Claim(ClaimTypes.Name, "User-1")
                }));
            });

            // EntityFramework
            services.AddScoped(provider =>
            {
                var connectionString = @"Data Source=(LocalDB)\mssqllocaldb;AttachDbFilename='|DataDirectory|\Application.mdf';Integrated Security=True"
                    .Replace("|DataDirectory|", new DirectoryInfo(
                        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)).GetParent(2).FullName);

                return new DataContext(
                    connectionString,
                    provider.GetService<ITimestampProvider>(),
                    provider.GetService<IPrincipal>()
                );
            });

            var serviceProvider = services.BuildServiceProvider();

            var app = serviceProvider.GetRequiredService<IApp>();

            app.RunAsync().Wait();
        }
    }
}
