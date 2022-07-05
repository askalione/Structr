using Microsoft.Extensions.DependencyInjection.Extensions;
using Structr.Notices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// <see cref="ServiceCollection"/> extension methods for configuring Notices services.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <inheritdoc cref="AddNotices(IServiceCollection, Action{NoticeServiceOptions}, Assembly[])"/>
        public static IServiceCollection AddNotices(this IServiceCollection services, params Assembly[] assembliesToScan)
            => AddNotices(services, null, assembliesToScan);

        /// <summary>
        /// Adds basic Notices services.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="configureOptions">The <see cref="NoticeServiceOptions"/> to be used by notices handling service.</param>
        /// <param name="assembliesToScan">Assembly to search <see cref="INoticeHandler{TNotice}"/>.</param>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="services"/> is <see langword="null"/>.</exception>
        public static IServiceCollection AddNotices(this IServiceCollection services,
            Action<NoticeServiceOptions> configureOptions,
            params Assembly[] assembliesToScan)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            var options = new NoticeServiceOptions();

            configureOptions?.Invoke(options);

            services.TryAdd(new ServiceDescriptor(typeof(INoticePublisher), options.PublisherServiceType, options.PublisherServiceLifetime));

            services.AddClasses(assembliesToScan);

            return services;
        }

        private static IServiceCollection AddClasses(this IServiceCollection services, params Assembly[] assembliesToScan)
        {
            if (assembliesToScan != null && assembliesToScan.Length > 0)
            {
                TypeInfo[] allTypes = assembliesToScan
                    .Where(a => !a.IsDynamic && a != typeof(INoticePublisher).Assembly)
                    .Distinct()
                    .SelectMany(a => a.DefinedTypes)
                    .ToArray();

                var openTypes = new[]
                {
                    typeof(INoticeHandler<>)
                };

                IEnumerable<TypeInfo> typeInfos = openTypes.SelectMany(openType => allTypes
                    .Where(t => t.IsClass
                             && t.IsGenericType == false
                             && t.IsAbstract == false
                             && t.AsType().ImplementsGenericInterface(openType)));

                foreach (var typeInfo in typeInfos)
                {
                    Type implementationType = typeInfo.AsType();

                    foreach (Type interfaceType in implementationType.GetInterfaces()
                        .Where(i => openTypes.Any(openType => i.ImplementsGenericInterface(openType))))
                    {
                        services.AddTransient(interfaceType, implementationType);
                    }
                }
            }

            return services;
        }

        private static bool ImplementsGenericInterface(this Type type, Type interfaceType)
            => type.IsGenericType(interfaceType) || type.GetTypeInfo().ImplementedInterfaces.Any(@interface => @interface.IsGenericType(interfaceType));

        private static bool IsGenericType(this Type type, Type genericType)
            => type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == genericType;
    }
}
