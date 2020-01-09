using Microsoft.Extensions.DependencyInjection.Extensions;
using Scrutor;
using Structr.Notices;
using System;
using System.Linq;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddNotices(this IServiceCollection services, params Assembly[] assembliesToScan)
            => AddNotices(services, null, assembliesToScan);

        public static IServiceCollection AddNotices(this IServiceCollection services,
            Action<NoticeServiceOptions> configureOptions,
            params Assembly[] assembliesToScan)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            if (assembliesToScan == null)
                throw new ArgumentNullException(nameof(assembliesToScan));
            if (!assembliesToScan.Any())
                throw new ArgumentException("No assemblies found to scan. At least one assembly to scan for handlers is required");

            var options = new NoticeServiceOptions();

            configureOptions?.Invoke(options);

            services.TryAdd(new ServiceDescriptor(typeof(INoticePublisher), options.PublisherType, options.Lifetime));

            services
                .Scan(opt => opt.FromAssemblies(assembliesToScan)
                    .AddClasses(classes => classes.AssignableTo(typeof(INoticeHandler<>)))
                    .UsingRegistrationStrategy(RegistrationStrategy.Append)
                    .AsImplementedInterfaces()
                    //.As(c => c.GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(INoticeHandler<>)))
                    .WithTransientLifetime());

            return services;
        }
    }
}
