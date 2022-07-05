using Microsoft.Extensions.DependencyInjection.Extensions;
using Structr.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// <see cref="ServiceCollection"/> extension methods for configuring Validation services.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <inheritdoc cref="AddValidation(IServiceCollection, Action{ValidationServiceOptions}, Assembly[])"/>
        public static IServiceCollection AddValidation(this IServiceCollection services, params Assembly[] assembliesToScan)
            => AddValidation(services, null, assembliesToScan);

        /// <summary>
        /// Adds basic Validation services.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="configureOptions">The <see cref="ValidationServiceOptions"/> to be used by validation service.</param>
        /// <param name="assembliesToScan">Assembly to search <see cref="IValidator{}"/>.</param>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="services"/> is <see langword="null"/>.</exception>
        public static IServiceCollection AddValidation(this IServiceCollection services,
            Action<ValidationServiceOptions> configureOptions,
            params Assembly[] assembliesToScan)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            var options = new ValidationServiceOptions();

            configureOptions?.Invoke(options);

            services.TryAdd(new ServiceDescriptor(typeof(IValidationProvider), options.ProviderServiceType, options.ProviderServiceLifetime));

            services.AddClasses(assembliesToScan);

            return services;
        }

        private static IServiceCollection AddClasses(this IServiceCollection services, params Assembly[] assembliesToScan)
        {
            if (assembliesToScan != null && assembliesToScan.Length > 0)
            {
                TypeInfo[] allTypes = assembliesToScan
                    .Where(a => a.IsDynamic == false && a != typeof(IValidationProvider).Assembly)
                    .Distinct()
                    .SelectMany(a => a.DefinedTypes)
                    .ToArray();

                Type[] openTypes = new[]
                {
                    typeof(IValidator<>)
                };

                IEnumerable<TypeInfo> typeInfos = openTypes.SelectMany(openType =>
                {
                    return allTypes
                        .Where(t => t.IsClass
                                 && t.IsGenericType == false
                                 && t.IsAbstract == false
                                 && t.AsType().ImplementsGenericInterface(openType));
                });

                foreach (TypeInfo typeInfo in typeInfos)
                {
                    Type implementationType = typeInfo.AsType();
                    IEnumerable<Type> interfaceTypes = implementationType
                        .GetInterfaces()
                        .Where(i => openTypes.Any(openType => i.ImplementsGenericInterface(openType)));
                    foreach (Type interfaceType in interfaceTypes)
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
