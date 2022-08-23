using Structr.Samples;
using Structr.Samples.IO;
using Structr.Samples.Timestamp;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSample<TApp>(this IServiceCollection services) where TApp : IApp
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            services.AddScoped(typeof(IApp), typeof(TApp));
            services.AddSingleton<IStringWriter>(new StringWriter(Console.Out));
            services.AddSingleton<ITimestampProvider, TimestampProvider>();

            return services;
        }
    }
}
