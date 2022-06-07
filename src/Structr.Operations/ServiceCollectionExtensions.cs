using Microsoft.Extensions.DependencyInjection.Extensions;
using Structr.Operations;
using System;
using System.Linq;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds operations handling service.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="assembliesToScan">List of assemblies to search operation handlers.</param>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddOperations(this IServiceCollection services, params Assembly[] assembliesToScan)
            => AddOperations(services, null, assembliesToScan);

        /// <summary>
        /// Adds operations handling service with provided configuration.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="configureOptions">Options to be used by operations handling service.</param>
        /// <param name="assembliesToScan">List of assemblies to search operation handlers.</param>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddOperations(this IServiceCollection services,
            Action<OperationServiceOptions> configureOptions,
            params Assembly[] assembliesToScan)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            var options = new OperationServiceOptions();

            configureOptions?.Invoke(options);

            services.TryAdd(new ServiceDescriptor(typeof(IOperationExecutor), options.ExecutorType, options.ExecutorServiceLifetime));

            services.AddHandlerClasses(assembliesToScan);

            return services;
        }

        private static IServiceCollection AddHandlerClasses(this IServiceCollection services, params Assembly[] assembliesToScan)
        {
            if (assembliesToScan != null && assembliesToScan.Length > 0)
            {
                var allTypes = assembliesToScan
                    .Where(a => !a.IsDynamic && a != typeof(IOperationExecutor).Assembly)
                    .Distinct()
                    .SelectMany(a => a.DefinedTypes)
                    .ToArray();

                var openTypes = new[]
                {
                    typeof(IOperationHandler<,>),
                    typeof(IOperationHandler<>)
                };

                foreach (var typeInfo in openTypes.SelectMany(openType => allTypes
                    .Where(t => t.IsClass
                        && !t.IsGenericType
                        && !t.IsAbstract
                        && t.AsType().ImplementsGenericInterface(openType))))
                {
                    var implementationType = typeInfo.AsType();
                    foreach (var interfaceType in implementationType.GetInterfaces()
                        .Where(i => openTypes.Any(openType => i.ImplementsGenericInterface(openType))))
                    {
                        services.TryAddTransient(interfaceType, implementationType);
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
